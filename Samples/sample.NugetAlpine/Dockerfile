# Build context: this folder (Samples/sample.NugetAlpine). No project references, NuGet packages only.
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG WONDERFUL_CAPTCHA_VERSION=10.3.0
WORKDIR /src
COPY . .
RUN dotnet publish sample.NugetAlpine.csproj -c Release -o /app -p:WonderfulCaptchaVersion=$WONDERFUL_CAPTCHA_VERSION

# Plain Alpine runtime: no fonts installed.
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app
COPY --from=build /app .
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "sample.NugetAlpine.dll"]
