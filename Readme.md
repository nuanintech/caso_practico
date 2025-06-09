# Microservicios del Caso Práctico

Este repositorio contiene una solución de ejemplo compuesta por dos microservicios en .NET 8: `client-service` y `task-service`. La pila también incluye MySQL, SQL Server, RabbitMQ, un servidor FTP y un API Gateway basado en Nginx. Se utiliza Docker Compose para el desarrollo y el despliegue locales.

## Prerrequisitos

- Docker y Docker Compose instalados
- Opcional: `git` para clonar el proyecto
- El archivo `docker-compose.yml` depende de un archivo `.env` ubicado en la raíz del proyecto. Ajusta los valores en `.env` para que coincidan con tu entorno (contraseñas de bases de datos, credenciales de RabbitMQ, etc.).

## Ejecución de la solución

1. Clona este repositorio y entra en el directorio:

   ```bash
   git clone <repo-url>
   cd caso_practico
   ```
2. Revisa las variables definidas en `.env` y modifícalas si es necesario. Estos valores se inyectan en los contenedores al iniciar. Como mínimo configura las contraseñas de las bases de datos, las credenciales de RabbitMQ y la información del host FTP.
3. Construye y levanta todos los contenedores:

   ```bash
   docker compose up --build
   ```

   La primera ejecución descargará las imágenes necesarias, construirá las imágenes de los dos servicios y levantará la infraestructura de soporte (bases de datos, RabbitMQ, servidor FTP y el API Gateway).
4. Una vez que todos los servicios estén en ejecución puedes acceder a las APIs a través del gateway en `http://localhost:4000`.

## Inicialización y migraciones de base de datos

Los contenedores de MySQL y SQL Server ejecutan automáticamente los scripts en `mysql/init.sql` y `sqlserver/init.sql` cuando arrancan. Estos scripts crean las tablas iniciales y cargan datos de ejemplo. Si necesitas aplicar cambios adicionales a la base de datos, modifica estos archivos SQL o ejecuta tus propios scripts contra los contenedores en funcionamiento utilizando un cliente SQL.

## Variables de entorno

El archivo `.env` provee todas las variables que utiliza `docker-compose.yml`. Allí puedes personalizar puertos, credenciales y otros ajustes. Los cambios en este archivo requieren recrear los contenedores (`docker compose up --build`) para que tengan efecto.

### Carpetas de logs
Cada servicio escribe sus registros en `/app/logs`. Estos directorios se persisten utilizando los volúmenes `client_logs` y `task_logs`. La variable de entorno `LOG_DIR` permite cambiar esta ruta.

### Carpeta FTP
El servidor FTP monta `./ftp/data:/home/vsftpd`. Dentro se encuentra la estructura `ftpuser/{error,procesados}` donde se almacenan los archivos recibidos. Las variables `FTP_FOLDER_PENDING`, `FTP_FOLDER_PROCESSED` y `FTP_FOLDER_ERROR` indican las carpetas de pendientes, procesados y errores respectivamente.

---

Después de editar las variables de entorno o los scripts de base de datos recuerda reconstruir los contenedores para que los cambios se apliquen.
