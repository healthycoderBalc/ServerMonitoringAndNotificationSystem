FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5009
EXPOSE 7296

COPY ./SignalRServer/SignalRServer.pfx /https/SignalRServer.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SignalRServer/SignalRServer.csproj", "SignalRServer/"]
RUN dotnet restore "./SignalRServer/SignalRServer.csproj"
COPY . .
WORKDIR "/src/SignalRServer"
RUN dotnet build "./SignalRServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SignalRServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRServer.dll"]