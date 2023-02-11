FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Nexttag.Authentication/Nexttag.Authentication.csproj", "Nexttag.Authentication/"]
RUN dotnet restore "Nexttag.Authentication/Nexttag.Authentication.csproj"
COPY . .
WORKDIR "/src/Nexttag.Authentication"
RUN dotnet build "Nexttag.Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexttag.Authentication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexttag.Authentication.dll"]
