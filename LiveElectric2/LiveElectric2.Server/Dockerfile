# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Устанавливаем Node.js для клиентской части
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Установка Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs

# Копируем csproj файлы и восстанавливаем зависимости для серверной части
COPY ["liveelectric2.client/nuget.config", "liveelectric2.client/"]
COPY ["LiveElectric2.Server/LiveElectric2.Server.csproj", "LiveElectric2.Server/"]
COPY ["liveelectric2.client/liveelectric2.client.esproj", "liveelectric2.client/"]
COPY ["LiveElectric2.ServiceDefaults/LiveElectric2.ServiceDefaults.csproj", "LiveElectric2.ServiceDefaults/"]
RUN dotnet restore "./LiveElectric2.Server/LiveElectric2.Server.csproj"

# Копируем все остальные файлы для сборки
COPY . .

# Сборка серверной части
WORKDIR "/src/LiveElectric2.Server"
RUN dotnet build "./LiveElectric2.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Сборка клиентской части
WORKDIR "/src/liveelectric2.client"
RUN npm install
RUN npm run build --prod

# Публикация серверной части
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/LiveElectric2.Server

# Копируем собранные файлы клиента в папку wwwroot
RUN mkdir -p /app/publish/wwwroot
COPY --from=build /src/liveelectric2.client/dist/* /app/publish/wwwroot/

RUN dotnet publish "./LiveElectric2.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ, который будет использоваться для запуска
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiveElectric2.Server.dll"]