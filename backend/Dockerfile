FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore SecretMission/*.csproj
RUN dotnet publish SecretMission/*.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env ./app/out ./
ENTRYPOINT ["dotnet", "SecretMission.dll"]
