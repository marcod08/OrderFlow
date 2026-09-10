FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/OrderingService/Ordering.Service.Api/Ordering.Service.Api.csproj", "src/OrderingService/Ordering.Service.Api/"]
RUN dotnet restore "src/OrderingService/Ordering.Service.Api/Ordering.Service.Api.csproj"
COPY . .
WORKDIR "/src/src/OrderingService/Ordering.Service.Api"
RUN dotnet build "Ordering.Service.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Ordering.Service.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Service.Api.dll"]
