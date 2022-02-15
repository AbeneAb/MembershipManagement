#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API/Membership/Membership.API/Membership.API.csproj", "API/Membership/Membership.API/"]
RUN dotnet restore "API/Membership/Membership.API/Membership.API.csproj"
COPY . .
WORKDIR "/src/API/Membership/Membership.API"
RUN dotnet build "Membership.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Membership.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Membership.API.dll"]