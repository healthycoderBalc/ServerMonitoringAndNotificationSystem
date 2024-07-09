FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SignalREventConsumerService/SignalREventConsumerService.csproj", "SignalREventConsumerService/"]
RUN dotnet restore "./SignalREventConsumerService/SignalREventConsumerService.csproj"
COPY . .
WORKDIR "/src/SignalREventConsumerService"
RUN dotnet build "./SignalREventConsumerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SignalREventConsumerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalREventConsumerService.dll"]