FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TeslamateAPIGetProxy/TeslamateAPIGetProxy.csproj", "TeslamateAPIGetProxy/"]
RUN dotnet restore "./TeslamateAPIGetProxy/TeslamateAPIGetProxy.csproj"
COPY . .
RUN dotnet build "./TeslamateAPIGetProxy/TeslamateAPIGetProxy.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TeslamateAPIGetProxy/TeslamateAPIGetProxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeslamateAPIGetProxy.dll"]