FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

WORKDIR /app

COPY . ./

RUN dotnet publish Contoso.Spaces.Populate.Storage/Contoso.Spaces.Populate.Storage.csproj --output out --configuration Release

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Contoso.Spaces.Populate.Storage.dll"]