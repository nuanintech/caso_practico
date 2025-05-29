-- Usar la base de datos deseada
USE users_db;

-- Crear tabla de usuarios con GUID como clave primaria
CREATE TABLE IF NOT EXISTS usuarios (
    id CHAR(36) PRIMARY KEY,
    identificacion VARCHAR(20) NOT NULL,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    edad INT,
    email VARCHAR(50) NOT NULL,
    cargo VARCHAR(50),
    estado VARCHAR(10) NOT NULL DEFAULT 'Activo',
    CONSTRAINT chk_estado_usuario CHECK (estado IN ('Activo', 'Inactivo'))
);

-- Insertar dos usuarios iniciales
INSERT INTO usuarios (id, identificacion, nombres, apellidos, edad, email, cargo, estado)
VALUES 
(UUID(), '0103767661', 'Cristian Andres', 'Moreno Cobos', 39, 'cristian.moreno1986@outlook.com','Desarrollador senior', 'Activo'),
(UUID(), '0301560280', 'Mayra de la Nube', 'Reinoso Lopez', 44, 'mreinoso_2001@hotmail.com', 'Desarrollador junior', 'Activo');