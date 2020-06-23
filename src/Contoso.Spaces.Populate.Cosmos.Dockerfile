FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

WORKDIR /app

COPY . ./

RUN dotnet publish Contoso.Spaces.Populate.Cosmos/Contoso.Spaces.Populate.Cosmos.csproj --output out --configuration Release

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine

COPY --from=build /app/out .

RUN apk add icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "Contoso.Spaces.Populate.Cosmos.dll"]