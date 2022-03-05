#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API/HealthData/HealthData.API/HealthData.API.csproj", "API/HealthData/HealthData.API/"]
COPY ["EventBus/RabbitMQEventbus/RabbitMQEventbus.csproj", "EventBus/RabbitMQEventbus/"]
RUN dotnet restore "API/HealthData/HealthData.API/HealthData.API.csproj"
COPY . .
WORKDIR "/src/API/HealthData/HealthData.API"
RUN dotnet build "HealthData.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthData.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthData.API.dll"]