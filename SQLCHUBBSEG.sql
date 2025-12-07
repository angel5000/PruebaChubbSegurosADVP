CREATE DATABASE chubbseguros; --crea la base da tos
GO

USE chubbseguros;--usa la base datos

--creacion de las tablas en ingreso de datos


CREATE TABLE SEGUROS (
    IDSEGURO INT IDENTITY(1,1) PRIMARY KEY,
    NMBRSEGURO VARCHAR(75) NOT NULL,
    CODSEGURO VARCHAR(10) NOT NULL UNIQUE,
    SUMASEGURADA DECIMAL(10,2) NOT NULL,
    PRIMA DECIMAL(10,2) NOT NULL,
	EDADMIN INT NOT NULL,
	EDADMAX INT NOT NULL,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    USRCreacion VARCHAR(50) NOT NULL DEFAULT 'SYSTEM',
    FechaActualizacion DATETIME NULL,
    USRActualizacion VARCHAR(50) NULL,
	UsuarioIP VARCHAR(50) NULL,
    Estado BIT NOT NULL DEFAULT 1
);


INSERT INTO SEGUROS 
(NMBRSEGURO, CODSEGURO, SUMASEGURADA, PRIMA, EDADMIN, EDADMAX,
 FechaCreacion, USRCreacion, FechaActualizacion, USRActualizacion, UsuarioIP, Estado)
VALUES 
('Gastos Medicos Mayores', 'GMYS432000', 50000.00, 65.50, 30, 64,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1),

('Accidentes Personales', 'ADPR321000', 25000.00, 15.00, 18, 70,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1),

('Seguro de vida personal', 'SVPL543000', 100000.00, 35.00, 18, 55,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1),

('Seguro de Hogar y contenidos', 'SGRC765000', 150000.00, 22.50, 22, 99,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1),

('Seguro Viajes', 'SRVJ987000', 30000.00, 25.00, 20, 70,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1),

('Seguro Dental', 'SGDT433000', 3000.00, 15.00, 12, 60,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1);

 SELECT*FROM SEGUROS

CREATE TABLE ASEGURADOS (
    IDASEGURADOS INT IDENTITY(1,1) PRIMARY KEY,
    CEDULA VARCHAR(10) NOT NULL UNIQUE,
    NMBRCOMPLETO VARCHAR(75) NOT NULL,
    TELEFONO VARCHAR(10) NULL,
    EDAD INT NOT NULL,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    USRCreacion VARCHAR(50) NOT NULL DEFAULT 'SYSTEM',
    FechaActualizacion DATETIME NULL,
    USRActualizacion VARCHAR(50) NULL,
	UsuarioIP VARCHAR(50) NULL,
    Estado BIT NOT NULL DEFAULT 1
);


INSERT INTO ASEGURADOS 
(CEDULA, NMBRCOMPLETO, TELEFONO, EDAD,
 FechaCreacion, USRCreacion, FechaActualizacion, USRActualizacion, UsuarioIP, Estado)
VALUES 
('0914526845', 'ANGEL DAVID VERGARA PAREDES', '0948548788', 25,
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1);
 select * from seg


CREATE TABLE USRASEGURADOS (
    IDUSRSEGUROS INT IDENTITY(1,1) PRIMARY KEY,
    CEDULAFK VARCHAR(10) NOT NULL,
    CODSEGUROFK VARCHAR(10) NOT NULL,
    FECHACONTRATASEGURO DATE NOT NULL DEFAULT GETDATE(),
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    USRCreacion VARCHAR(50) NOT NULL DEFAULT 'SYSTEM',
    FechaActualizacion DATETIME NULL,
    USRActualizacion VARCHAR(50) NULL,
	UsuarioIP VARCHAR(50) NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (CEDULAFK) REFERENCES ASEGURADOS(CEDULA),
    FOREIGN KEY (CODSEGUROFK) REFERENCES SEGUROS(CODSEGURO)
);


INSERT INTO USRASEGURADOS 
(CEDULAFK, CODSEGUROFK,
 FechaCreacion, USRCreacion, FechaActualizacion, USRActualizacion, UsuarioIP, Estado)
VALUES 
('0914526845', 'SRVJ987000',
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1);




 CREATE TABLE Roles (
    IdRol INT PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200) NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);


INSERT INTO Roles (IdRol, NombreRol, Descripcion)
VALUES 
(100,'ADMINISTRADOR', 'Acceso total al sistema y gestión de seguridad'),
(101,'GESTOR_COBRANZAS', 'Gestiona pagos, facturación y cobranzas'),
(102,'CLIENTE', 'Accede a su portal para ver sus pólizas y pagos'),
(103,'AUDITOR_INTERNO', 'Revisa trazabilidad, controles y operaciones internas')


CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
	Cedula VARCHAR(10) NOT NULL UNIQUE,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARBINARY(256) ,
    PasswordSalt VARBINARY(256) ,
    Estado BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    UltimoLogin DATETIME NULL
);



DECLARE @Password NVARCHAR(200) = '123456';
DECLARE @Salt VARBINARY(64) = CRYPT_GEN_RANDOM(64);

-- HASH = HASH(SALT + PASSWORD)
DECLARE @Hash VARBINARY(64);
SET @Hash = HASHBYTES('SHA2_512', @Salt + CONVERT(VARBINARY(200), @Password));

UPDATE Usuarios
SET PasswordSalt = @Salt,
    PasswordHash = @Hash
WHERE IdUsuario = 3;

SELECT * FROM Usuarios
SELECT*FROM Roles



INSERT INTO Usuarios (Cedula, NombreUsuario, Correo, PasswordHash, PasswordSalt, Estado)
VALUES
('0948548555','admin.master', 'admin@aseguradora.com', NULL, NULL, 1),
('0933147789','cobranzas.mario', 'mario.cobranzas@aseguradora.com', NULL, NULL, 1),
('0998445331','cliente.juan', 'juan.cliente@gmail.com', NULL, NULL, 1),
('0946487111','auditor.carla', 'carla.auditoria@aseguradora.com', NULL, NULL, 1)



CREATE TABLE UsuarioRoles (
    IdUsuarioRol INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdRol INT NOT NULL,
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_UsuarioRol_Usuario FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_UsuarioRol_Rol FOREIGN KEY (IdRol)
        REFERENCES Roles(IdRol)
);



INSERT INTO UsuarioRoles (IdUsuario, IdRol)
VALUES (2, 100);

INSERT INTO UsuarioRoles (IdUsuario, IdRol)
VALUES (3, 101);
-- Índices recomendados:
CREATE INDEX IDX_UsuarioRoles_IdUsuario ON UsuarioRoles(IdUsuario);
CREATE INDEX IDX_UsuarioRoles_IdRol ON UsuarioRoles(IdRol);






 ---------------------------------
	---------HISTORICOS---------
 --------------------------------
CREATE TABLE SEGUROS_HISTORICO (
    IDHISTORICO INT IDENTITY(1,1) PRIMARY KEY,
    IDSEGURO INT,
    NMBRSEGURO VARCHAR(75),
    CODSEGURO VARCHAR(10),
    SUMASEGURADA DECIMAL(10,2),
    PRIMA DECIMAL(10,2),
    EDADMIN INT,
    EDADMAX INT,
    EstadoDT VARCHAR(15),
    FechaModElim DATETIME,
    UsuarioIP VARCHAR(50),
    USRActualizacion VARCHAR(50),
);

SELECT * FROM SEGUROS_HISTORICO