# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Instala Node.js para compilar TypeScript
RUN apt-get update && \
    apt-get install -y nodejs npm && \
    rm -rf /var/lib/apt/lists/*

# Copia los archivos de NPM y restaura paquetes
COPY package*.json ./
RUN npm ci

# Copia el archivo del proyecto y restaura paquetes de .NET
COPY FlightPlanner.csproj ./
RUN dotnet restore

# Copia el resto del código fuente
COPY . .
WORKDIR "/src/FlightPlanner"
RUN dotnet build "./FlightPlanner.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica la app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FlightPlanner.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app

# Copia los archivos publicados
COPY --from=build /app/publish .

# Usa el entrypoint
ENTRYPOINT ["dotnet", "FlightPlanner.dll"]
