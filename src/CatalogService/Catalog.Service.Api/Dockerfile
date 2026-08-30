FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/CatalogService/Catalog.Service.Api/Catalog.Service.Api.csproj", "src/CatalogService/Catalog.Service.Api/"]
RUN dotnet restore "src/CatalogService/Catalog.Service.Api/Catalog.Service.Api.csproj"
COPY . .
WORKDIR "/src/src/CatalogService/Catalog.Service.Api"
RUN dotnet build "Catalog.Service.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Catalog.Service.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Service.Api.dll"]
