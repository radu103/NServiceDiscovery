FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8771
EXPOSE 8771

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NServiceDiscoveryAPI/NServiceDiscoveryAPI.csproj", "NServiceDiscoveryAPI/"]
COPY ["NServiceDiscovery/NServiceDiscovery.csproj", "NServiceDiscovery/"]
RUN dotnet restore "NServiceDiscoveryAPI/NServiceDiscoveryAPI.csproj"
COPY . .
WORKDIR "/src/NServiceDiscoveryAPI"
RUN dotnet build "NServiceDiscoveryAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NServiceDiscoveryAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NServiceDiscoveryAPI.dll"]