# 1. Use the .NET SDK image to build the worker
FROM mcr.microsoft.com/dotnet/sdk:9.0.301-bookworm-slim AS build
WORKDIR /src

# Copy solution and project files (no trailing slashes)
COPY BattleBunnies.sln ./
COPY BattleBunnies.Domain/BattleBunnies.Domain.csproj BattleBunnies.Domain/
COPY BattleBunnies.Contracts/BattleBunnies.Contracts.csproj BattleBunnies.Contracts/
COPY BattleBunnies.Infrastructure/BattleBunnies.Infrastructure.csproj BattleBunnies.Infrastructure/
COPY BattleBunnies.EmailConfirmationMS/BattleBunnies.EmailConfirmationMS.csproj BattleBunnies.EmailConfirmationMS/

# Restore dependencies
RUN dotnet restore BattleBunnies.EmailConfirmationMS/BattleBunnies.EmailConfirmationMS.csproj
# Copy the rest of the source
COPY . .

# Publish the worker
RUN dotnet publish BattleBunnies.EmailConfirmationMS/BattleBunnies.EmailConfirmationMS.csproj -c Release -o /app/publish

# 2. Use the ASP.NET runtime image to run the worker
FROM mcr.microsoft.com/dotnet/aspnet:9.0.6-bookworm-slim AS final
WORKDIR /app
COPY --from=build /app/publish .

# Start the worker
ENTRYPOINT ["dotnet", "BattleBunnies.EmailConfirmationMS.dll"]
