# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 10080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["IMDB.WEB.API/IMDB.WEB.API.csproj", "IMDB.WEB.API/"]
COPY ["../IMDB.Infrastructure/IMDB.Infrastructure.csproj", "IMDB.Infrastructure/"]
COPY ["../IMDB.Application/IMDB.Application.csproj", "IMDB.Application/"]
COPY ["../IMDB.Domain/IMDB.Domain.csproj", "IMDB.Domain/"]

RUN dotnet restore "./IMDB.WEB.API/IMDB.WEB.API.csproj"

COPY . .

WORKDIR "/src/IMDB.WEB.API"
RUN dotnet build "./IMDB.WEB.API.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IMDB.WEB.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IMDB.WEB.API.dll"]