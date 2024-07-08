FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MessageProcessingAndAnomalyDetection/MessageProcessingAndAnomalyDetection.csproj", "MessageProcessingAndAnomalyDetection/"]
RUN dotnet restore "./MessageProcessingAndAnomalyDetection/MessageProcessingAndAnomalyDetection.csproj"

COPY . .
WORKDIR "/src/MessageProcessingAndAnomalyDetection"
RUN dotnet build "./MessageProcessingAndAnomalyDetection.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MessageProcessingAndAnomalyDetection.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .

ENV RABBITMQ_HOSTNAME=localhost
ENV RABBITMQ_USERNAME=guest
ENV RABBITMQ_PASSWORD=guest
ENV MONGODB_CONNECTIONSTRING="mongodb+srv://mmrbalcazar:FBAc8KKJ0DMCvJWk@cluster0.tix8qz7.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0"
ENV MONGODB_DATABASENAME=ServerStatisticsDB
ENV MONGODB_COLLECTIONNAME=Statistics
ENV SIGNALR_URL=https://localhost:7296/alertHub
ENV MEMORY_USAGE_ANOMALY_THRESHOLD_PERCENTAGE=0.4
ENV CPU_USAGE_ANOMALY_THRESHOLD_PERCENTAGE=0.5
ENV MEMORY_USAGE_THRESHOLD_PERCENTAGE=0.8
ENV CPU_USAGE_THRESHOLD_PERCENTAGE=0.9

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "MessageProcessingAndAnomalyDetection.dll"]
