# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Api/Api.csproj", "Api/"]
RUN dotnet restore "./Api/Api.csproj"

COPY . .
WORKDIR "/src/Api"
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Render sends traffic to $PORT (default 10000) and expects binding on 0.0.0.0
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

COPY --from=build /app/publish .
EXPOSE 10000

ENTRYPOINT ["dotnet", "Api.dll"]