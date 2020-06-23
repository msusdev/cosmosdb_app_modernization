FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base

WORKDIR /app

COPY . ./

RUN mkdir -p /home/site/wwwroot

RUN dotnet publish Contoso.Spaces.Api.Sql/Contoso.Spaces.Api.Sql.csproj --output /home/site/wwwroot --configuration Release

FROM mcr.microsoft.com/azure-functions/dotnet:3.0

ENV AzureWebJobsScriptRoot=/home/site/wwwroot

ENV AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=base /home/site/wwwroot /home/site/wwwroot