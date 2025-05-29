USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'tasks_db')
BEGIN
    CREATE DATABASE tasks_db;
END
GO

USE tasks_db;
GO

-- Crear tabla tareas si no existe
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tareas'
)
BEGIN
    CREATE TABLE tareas (
        id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        codigotarea VARCHAR(50) NOT NULL,
        titulo VARCHAR(100) NOT NULL,
        descripcion TEXT,
        criteriosaceptacion TEXT,
        fechainicio DATE,
        fechafin DATE,
        tiempodias INT,
        estadotarea VARCHAR(20) NOT NULL,
        estado VARCHAR(10) DEFAULT 'Activo',
        usuarioid CHAR(36) NULL,
        CONSTRAINT CK_EstadoTarea CHECK (estadotarea IN ('Backlog', 'Doing', 'InReview', 'Done')),
        CONSTRAINT CK_Estado CHECK (estado IN ('Activo', 'Inactivo'))
    );
END
GO

-- Insertar tareas de ejemplo solo si la tabla existe y no tiene registros
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tareas'
)
AND NOT EXISTS (
    SELECT 1 FROM tareas
)
BEGIN
    INSERT INTO tareas (
        id, codigotarea, titulo, descripcion, criteriosaceptacion,
        fechainicio, fechafin, tiempodias, estadotarea, estado, usuarioid
    )
    VALUES 
    (NEWID(), 'TAR-001', 'Diseñar base de datos', 'Definir estructura inicial', 'Base con claves primarias y foráneas', 
     '2025-05-01', '2025-05-03', 2, 'Backlog', 'Activo', NULL),

    (NEWID(), 'TAR-002', 'Crear API de usuarios', 'Endpoints de CRUD', 'Operaciones con Dapper y validación', 
     '2025-05-04', '2025-05-06', 2, 'Doing', 'Activo', NULL),

    (NEWID(), 'TAR-003', 'Pruebas unitarias', 'Cobertura mínima del 80%', 'Todos los métodos deben tener test', 
     '2025-05-07', '2025-05-08', 1, 'InReview', 'Inactivo', NULL);
END
GO
