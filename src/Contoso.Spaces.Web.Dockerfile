FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

EXPOSE 80

WORKDIR /app

COPY . ./

RUN dotnet publish Contoso.Spaces.Web/Contoso.Spaces.Web.csproj --output out --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Contoso.Spaces.Web.dll"]