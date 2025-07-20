# .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0.301-bookworm-slim AS build
WORKDIR /src

# Copy the solution and projects
COPY BattleBunnies.sln ./
COPY BattleBunnies.Api/BattleBunnies.Api.csproj BattleBunnies.Api/
COPY BattleBunnies.Application/BattleBunnies.Application.csproj BattleBunnies.Application/
COPY BattleBunnies.Domain/BattleBunnies.Domain.csproj BattleBunnies.Domain/
COPY BattleBunnies.Infrastructure/BattleBunnies.Infrastructure.csproj BattleBunnies.Infrastructure/
COPY BattleBunnies.Contracts/BattleBunnies.Contracts.csproj BattleBunnies.Contracts/

# Restore dependencies
RUN dotnet restore BattleBunnies.Api/BattleBunnies.Api.csproj

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet publish BattleBunnies.Api/BattleBunnies.Api.csproj -c Release -o /app/publish

# Use the official ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0.6-bookworm-slim AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "BattleBunnies.Api.dll"]
