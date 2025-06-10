# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

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

# Publica la app
RUN dotnet publish -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app

# Copia los archivos publicados
COPY --from=build /app/publish .

# Usa el entrypoint
ENTRYPOINT ["dotnet", "FlightPlanner.dll"]
