#!/bin/bash
set -e

#######################################
# 📄 Load YAML Config
#######################################
CONFIG_FILE="env.yaml"

parse_yaml() {
  local prefix=$1
  local s='[[:space:]]*'
  local w='[a-zA-Z0-9_]*'
  local fs
  fs=$(echo @|tr @ '\034')
  while IFS= read -r line; do
    if [[ "$line" =~ ^[[:space:]]*# ]] || [[ -z "$line" ]]; then
      continue
    fi
    if [[ "$line" =~ ^([a-zA-Z0-9_]+):[[:space:]]*(.*)$ ]]; then
      key="${BASH_REMATCH[1]}"
      value="${BASH_REMATCH[2]}"
      eval ${prefix}${key}="\"${value}\""
    fi
  done < "$CONFIG_FILE"
}

parse_yaml ""

#######################################
# 🔧 CONFIGURATION
#######################################
POD_NAME="battlebunnies-pod"
POSTGRES_CONTAINER="battlebunnies-postgres"
RABBITMQ_CONTAINER="battlebunnies-rabbitmq"
REDIS_CONTAINER="battlebunnies-redis"
API_CONTAINER="battlebunnies-api"
EMAIL_CONTAINER="email-confirmation-ms"

#######################################
# 🧱 POD + INFRASTRUCTURE
#######################################
bootstrap_pod_and_infra() {
  echo "🚪 Clearing port 5432"
  sudo kill -9 $(sudo lsof -t -i:5432) 2>/dev/null || echo "Port 5432 already free"

  if sudo podman pod exists $POD_NAME; then
    echo "🗑️ Removing existing pod '$POD_NAME'..."
    sudo podman pod rm -f $POD_NAME
  fi

  echo "🚀 Creating pod..."
  sudo podman pod create --name $POD_NAME -p 5276:80 -p 5432:5432 -p 15672:15672

  echo "🐘 Starting PostgreSQL..."
  sudo podman run -d --pod $POD_NAME --name $POSTGRES_CONTAINER \
    -e POSTGRES_USER="$postgres_user" \
    -e POSTGRES_PASSWORD="$postgres_password" \
    -e POSTGRES_DB="$postgres_db" \
    -v postgres_data:/var/lib/postgresql/data \
    docker.io/library/postgres:16

  echo "📬 Starting RabbitMQ..."
  sudo podman run -d \
    --pod $POD_NAME \
    --name $RABBITMQ_CONTAINER \
    -v rabbitmq_data:/var/lib/rabbitmq \
    docker.io/library/rabbitmq:3-management

  echo "🔁 Starting Redis..."
  sudo podman run -d \
    --pod $POD_NAME \
    --name $REDIS_CONTAINER \
    docker.io/library/redis:7

  echo "⏱️ Waiting 10 seconds for services to be ready..."
  sleep 10
}

ensure_infra_running() {
  if ! sudo podman pod exists $POD_NAME; then
    echo "❌ Pod '$POD_NAME' does not exist. Please run with --all first to initialize the environment."
    exit 1
  fi

  echo "✅ Ensuring infrastructure containers are running..."

  for container in $POSTGRES_CONTAINER $RABBITMQ_CONTAINER $REDIS_CONTAINER; do
    if ! sudo podman container exists "$container"; then
      echo "❌ Container $container not found. You must re-run with --all."
      exit 1
    fi

    STATUS=$(sudo podman inspect -f '{{.State.Status}}' "$container")
    if [[ "$STATUS" != "running" ]]; then
      echo "🔄 Starting $container..."
      sudo podman start "$container"
    fi
  done
}

ensure_container_running() {
  local container=$1
  if ! sudo podman container exists "$container"; then
    echo "❌ Container $container not found. You must re-run with --all."
    exit 1
  fi

  STATUS=$(sudo podman inspect -f '{{.State.Status}}' "$container")
  if [[ "$STATUS" != "running" ]]; then
    echo "🔄 Starting $container..."
    sudo podman start "$container"
  fi
}

#######################################
# 📂 DATABASE MIGRATIONS
#######################################
run_migrations() {
  echo "📂 Creating run-migrations container..."
  sudo podman create \
    --name run-migrations \
    --pod $POD_NAME \
    -e ConnectionStrings__Default="Host=localhost;Port=5432;Database=$postgres_db;Username=$postgres_user;Password=$postgres_password" \
    -v "$(pwd)":/src:Z \
    -w /src \
    mcr.microsoft.com/dotnet/sdk:9.0 \
    sh -c "dotnet tool install -g dotnet-ef && export PATH=\$PATH:/root/.dotnet/tools && dotnet ef database update --project BattleBunnies.Infrastructure --startup-project BattleBunnies.Api"

  echo "📱 Running migrations..."
  sudo podman start -a run-migrations
  echo "🧹 Cleaning up run-migrations..."
  sudo podman rm run-migrations
}

#######################################
# 🚀 API REBUILD
#######################################
build_api() {
  echo "💨 Rebuilding API..."
  ensure_container_running $EMAIL_CONTAINER

  if sudo podman container exists $API_CONTAINER; then
    echo "🗑️ Removing existing API container..."
    sudo podman rm -f $API_CONTAINER
  fi

  echo "⚙️ Building API image..."
  sudo podman build -t battlebunnies-api -f BattleBunnies.Api/Dockerfile .

  echo "🚀 Running API container..."
  sudo podman run -d \
    --pod $POD_NAME \
    --name $API_CONTAINER \
    -e ConnectionStrings__Default="Host=localhost;Port=5432;Database=$postgres_db;Username=$postgres_user;Password=$postgres_password" \
    -e RabbitMq__Host=localhost \
    battlebunnies-api
}

#######################################
# ✉️ EMAIL CONFIRMATION SERVICE
#######################################
build_email_ms() {
  echo "📌 Rebuilding Email Confirmation MS..."
  ensure_container_running $API_CONTAINER


  if sudo podman container exists $EMAIL_CONTAINER; then
    echo "🗑️ Removing existing EmailConfirmationMS container..."
    sudo podman rm -f $EMAIL_CONTAINER
  fi

  echo "📆 Building EmailConfirmationMS image..."
  sudo podman build -t email-confirmation-ms -f BattleBunnies.EmailConfirmationMS/Dockerfile .

  echo "🚀 Running EmailConfirmationMS container..."
  sudo podman run -d \
    --pod $POD_NAME \
    --name $EMAIL_CONTAINER \
    -e SMTP__Host="$smtp_host" \
    -e SMTP__Port="$smtp_port" \
    -e SMTP__Username="$smtp_username" \
    -e SMTP__Password="$smtp_password" \
    -e SMTP__From="$smtp_from" \
    -e Redis__ConnectionString="$redis_connection_string" \
    -e Confirmation__ConfirmationBaseURL="$confirmation_base_url" \
    -e Confirmation__CodeSecretKey="$confirmation_code_secret_key" \
    email-confirmation-ms
}

#######################################
# 🧭 OPTION PARSING
#######################################
case "$1" in
  --all)
    bootstrap_pod_and_infra
    run_migrations
    build_api
    build_email_ms
    ;;
  --api)
    ensure_infra_running
    build_api
    ;;
  --email)
    ensure_infra_running
    build_email_ms
    ;;
  *)
    echo "Usage: $0 [--all | --api | --email]"
    exit 1
    ;;
esac

echo "✅ BattleBunnies environment is up and running!"
