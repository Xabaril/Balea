#Docker file for Contoso University configuration demo.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.8-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.402-buster AS build
WORKDIR /src
COPY ./NuGet.config ./NuGet.config
COPY ./Directory.Build.props ./Directory.Build.props
COPY ./Directory.Build.targets ./Directory.Build.targets
COPY ./build ./build
COPY ./src ./src
COPY ./sample ./sample
WORKDIR /src/sample/ContosoUniversity.Configuration.Store
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContosoUniversity.Configuration.Store.dll"]