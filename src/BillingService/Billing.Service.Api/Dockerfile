FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/BillingService/Billing.Service.Api/Billing.Service.Api.csproj", "src/BillingService/Billing.Service.Api/"]
RUN dotnet restore "src/BillingService/Billing.Service.Api/Billing.Service.Api.csproj"
COPY . .
WORKDIR "/src/src/BillingService/Billing.Service.Api"
RUN dotnet build "Billing.Service.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Billing.Service.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Billing.Service.Api.dll"]
