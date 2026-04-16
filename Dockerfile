# ============================================================
#  Recruitment.Clean — Multi-stage Docker Build
#  Produces two targets: "web" and "api"
# ============================================================

# ------------------ Stage 1: Build --------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js 20 for Tailwind CSS build
RUN apt-get update && \
    apt-get install -y ca-certificates curl gnupg && \
    mkdir -p /etc/apt/keyrings && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    rm -rf /var/lib/apt/lists/*

# Copy project files first (for layer caching on restore)
COPY Recruitment.Clean.sln .
COPY Recruitment.Domain/Recruitment.Domain.csproj Recruitment.Domain/
COPY Recruitment.Application/Recruitment.Application.csproj Recruitment.Application/
COPY Recruitment.Infrastructure/Recruitment.Infrastructure.csproj Recruitment.Infrastructure/
COPY Recruitment.Web/Recruitment.Web.csproj Recruitment.Web/
COPY Recruitment.Api/Recruitment.Api.csproj Recruitment.Api/

# Restore NuGet packages
RUN dotnet restore Recruitment.Clean.sln

# Copy everything else
COPY . .

# Build Tailwind CSS
WORKDIR /src/Recruitment.Web
RUN npm ci && npm run build:css

# Publish Web project
WORKDIR /src
RUN dotnet publish Recruitment.Web/Recruitment.Web.csproj \
    -c Release -o /app/web --no-restore

# Publish API project
RUN dotnet publish Recruitment.Api/Recruitment.Api.csproj \
    -c Release -o /app/api --no-restore

# ------------------ Stage 2: Web Runtime -------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS web
WORKDIR /app

# Create uploads directory
RUN mkdir -p /app/uploads/cv

COPY --from=build /app/web .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Recruitment.Web.dll"]

# ------------------ Stage 3: API Runtime -------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app

# Create uploads directory
RUN mkdir -p /app/uploads/cv

COPY --from=build /app/api .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Recruitment.Api.dll"]
