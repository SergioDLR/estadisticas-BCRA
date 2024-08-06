FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PrincipalesVariables/PrincipalesVariables.csproj", "PrincipalesVariables/"]
RUN dotnet restore "PrincipalesVariables/PrincipalesVariables.csproj"
COPY . .
WORKDIR "/src/PrincipalesVariables"
RUN dotnet build "PrincipalesVariables.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PrincipalesVariables.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrincipalesVariables.dll"]
