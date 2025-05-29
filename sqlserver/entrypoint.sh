#!/bin/bash

# Verifica que la variable esté definida
if [ -z "$SA_PASSWORD" ]; then
  echo "ERROR: La variable SA_PASSWORD no está definida."
  exit 1
fi

# Iniciar SQL Server en segundo plano
/opt/mssql/bin/sqlservr &

# Esperar hasta que SQL Server esté disponible
echo "⏳ Esperando que SQL Server esté listo..."
sleep 5  # pequeño delay inicial

for i in {1..30}; do
  /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U SA -P "$SA_PASSWORD" -Q "SELECT 1;" &>/dev/null && break
  echo "🔄 Intento $i: SQL Server no está listo aún..."
  sleep 2
done

echo "SQL Server está listo."

# Ejecutar el script de inicialización
if [ -f /init.sql ]; then
  echo "Ejecutando script init.sql..."
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -d master -i /init.sql
else
  echo "No se encontró el archivo /init.sql, se omite la ejecución."
fi

# Mantener el proceso principal vivo
wait
