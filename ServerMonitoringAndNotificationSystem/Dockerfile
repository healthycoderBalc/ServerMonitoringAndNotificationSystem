FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ServerMonitoringAndNotificationSystem/ServerMonitoringAndNotificationSystem.csproj", "ServerMonitoringAndNotificationSystem/"]
RUN dotnet restore "./ServerMonitoringAndNotificationSystem/ServerMonitoringAndNotificationSystem.csproj"

COPY . .
WORKDIR "/src/ServerMonitoringAndNotificationSystem"
RUN dotnet build "./ServerMonitoringAndNotificationSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ServerMonitoringAndNotificationSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .

ENV RABBITMQ_HOSTNAME=localhost
ENV RABBITMQ_USERNAME=guest
ENV RABBITMQ_PASSWORD=guest
ENV SERVER_IDENTIFIER=linux1
ENV SAMPLING_INTERVAL_SECONDS=60

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "ServerMonitoringAndNotificationSystem.dll"]
