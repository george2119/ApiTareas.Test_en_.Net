# 1. Etapa de Compilación (SDK de .NET 9)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y publicar la app
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Etapa de Ejecución (Runtime de .NET 9)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto por defecto de ASP.NET Core
EXPOSE 8080
ENTRYPOINT ["dotnet", "ApiTareas.dll"]