# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Etapa base: imagen liviana del runtime de ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Usa el usuario root directamente (por defecto)
USER root
# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app
# Expone el puerto HTTP que usará la app (ajústalo si usás otro)
EXPOSE 8080


# Esta fase se usa para compilar el proyecto de servicio
# Etapa build: imagen con SDK para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Define una variable de entorno para el tipo de compilación
ARG BUILD_CONFIGURATION=Release
# Establece el directorio de trabajo para la compilación
WORKDIR /src
# Copia solo el archivo .csproj para restaurar dependencias primero (mejor caché)
COPY ["client-service/client-service.csproj", "client-service/"]
# Restaura paquetes NuGet
RUN dotnet restore "./client-service/client-service.csproj"
# Copia el resto del código fuente
COPY . .
# Establece el contexto al proyecto real
WORKDIR "/src/client-service"
# Compila el proyecto
RUN dotnet build "./client-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish: publica solo los archivos necesarios para producción
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./client-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final: imagen liviana solo con runtime (sin SDK)
FROM base AS final
# Establece el directorio donde se ejecutará la app
WORKDIR /app
# Copia los archivos publicados desde la etapa anterior
COPY --from=publish /app/publish .
# Crea un directorio para logs
RUN mkdir -p /app/logs
# Ejecutar como root
USER root
# Define el punto de entrada del contenedor
ENTRYPOINT ["dotnet", "client-service.dll"]
