# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["FIAP.PosTech.ArqSistemas.Notification/FIAP.PosTech.ArqSistemas.NotificationWorkerService/FIAP.PosTech.ArqSistemas.NotificationWS.csproj", "FIAP.PosTech.ArqSistemas.Notification/FIAP.PosTech.ArqSistemas.NotificationWorkerService/"]
RUN dotnet restore "./FIAP.PosTech.ArqSistemas.Notification/FIAP.PosTech.ArqSistemas.NotificationWorkerService/FIAP.PosTech.ArqSistemas.NotificationWS.csproj"

COPY . .

WORKDIR "/src/FIAP.PosTech.ArqSistemas.Notification/FIAP.PosTech.ArqSistemas.NotificationWorkerService"

RUN dotnet build "./FIAP.PosTech.ArqSistemas.NotificationWS.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.PosTech.ArqSistemas.NotificationWS.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.PosTech.ArqSistemas.NotificationWS.dll"]