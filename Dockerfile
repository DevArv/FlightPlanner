# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo del proyecto y restaura
COPY ["FlightPlanner.csproj", "./"]
RUN dotnet restore

# Copia todo el contenido del proyecto
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
