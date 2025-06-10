# Sistema de Gestión de Tareas (Microservicios con .NET Core)

## Introducción
Este proyecto implementa un sistema escalable, resiliente y mantenible para la gestión interna de tareas en la empresa XYZ, usando una arquitectura basada en microservicios, específicamente con dos servicios principales: 
- `client-service`: Gestión de usuarios.
- `task-service`: Gestión y asignación de tareas.

## Arquitectura del Proyecto

- Arquitectura basada en microservicios desacoplados.
- API REST desarrollada en .NET Core.
- Comunicación asíncrona mediante RabbitMQ.
- Docker para despliegue y gestión de contenedores.
- NGINX actuando como API Gateway.

## Componentes Principales

### Microservicio de Usuarios (`client-service`)
- Manejo de la información de usuarios.
- Operaciones CRUD (Create, Read, Update, Delete).
- Base de datos: MySQL.

### Microservicio de Tareas (`task-service`)
- Gestión completa de tareas.
- Asignación de tareas a usuarios.
- Operaciones CRUD.
- Base de datos: SQL Server.
- Integración con FTP para carga y lectura de archivos JSON.

## Requisitos Técnicos

- .NET Core (versión 8.x)
- Docker y Docker Compose
- SQL Server y MySQL en contenedores Docker
- RabbitMQ (contenedor Docker)
- NGINX (contenedor Docker como API Gateway)
- FTP simulado (contenedor Docker)

## Instalación y Despliegue

### Requisitos previos
- Tener instalado Docker 

### Pasos para levantar el proyecto

1. **Clonar repositorio**:
   ```bash
   git clone https://github.com/nuanintech/caso_practico.git
   cd caso_practico

2. Configurar el entorno:
El archivo `.env` provee todas las variables que utiliza `docker-compose.yml`. Allí puedes personalizar puertos, credenciales y otros ajustes. Los cambios en este archivo requieren recrear los contenedores (`docker compose up --build`) para que tengan efecto.


3. Levantar los contenedores:
   ```bash
   docker-compose up -d
   ```

4. Verificar ejecución:
   ```bash
   docker-compose ps
   ```
## Documentación del API

Documentación completa en Postman, ubicada en `/postman`.

## Diagramas Disponibles

- Diagrama C4 (Contexto, Contenedores, Componentes)
- Diagrama de Arquitectura General
- Diagrama de Componentes (`task-service`)
- Diagrama de Secuencia (`task-service`)

Diagramas en `/docs/diagramas`.

## Base de Datos

Scripts SQL en:
- `/sqlserver` (tareas)
- `/mysql` (usuarios)

## Pruebas

Para ejecutar las pruebas desde la terminal:

```bash
cd task-service-test
dotnet test
```


### Carpetas de logs
Cada servicio escribe sus registros en `/app/logs`. Estos directorios se persisten utilizando los volúmenes `client_logs` y `task_logs`. La variable de entorno `LOG_DIR` permite cambiar esta ruta.

### Carga de tareas por FTP
Para crear tareas automáticamente, coloque un archivo JSON en la carpeta definida por `FTP_FOLDER_PENDING`. El servicio `FtpTareaProcesarServicio` del `task-service` revisa periódicamente esa ubicación y registra cada tarea que siga la estructura `CreateTareaDTO`. Tras el procesamiento, los resultados se guardan en las rutas configuradas por `FTP_FOLDER_PROCESSED` o `FTP_FOLDER_ERROR`.

---

Después de editar las variables de entorno o los scripts de base de datos recuerda reconstruir los contenedores para que los cambios se apliquen.
