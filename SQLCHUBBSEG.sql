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
 select * from ASEGURADOS


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
('0914526845', 'ADPR321000',
 GETDATE(), 'SYSTEM', NULL, NULL, '127.0.0.1', 1);


 SELECT * FROM ASEGURADOS

 DELETE FROM ASEGURADOS WHERE IDASEGURADOS  BETWEEN 3 AND 9

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

SELECT * FROM COBRANZAS



CREATE TABLE COBRANZAS (
    IDCOBRANZA INT IDENTITY(1,1) PRIMARY KEY,
    
    -- Relación con la póliza contratada (De aquí sacamos Cliente y Seguro)
    IDUSRSEGUROSFK INT NOT NULL, 
    
    -- Datos Financieros de la Deuda
    FECHA_EMISION DATE NOT NULL DEFAULT GETDATE(), -- Cuándo se generó el cobro
    FECHA_VENCIMIENTO DATE NOT NULL,               -- Fecha límite (Para calcular Mora)
    MONTO_ESPERADO DECIMAL(10,2) NOT NULL,         -- Valor que debe pagar (Prima)
    
    -- Datos del Pago (Se llenan cuando el gestor da clic en "COBRAR")
    MONTO_PAGADO DECIMAL(10,2) DEFAULT 0,
    FECHA_PAGO DATETIME NULL,
    METODO_PAGO VARCHAR(50) NULL,      -- 'EFECTIVO', 'TRANSFERENCIA', 'CHEQUE', etc.
    REFERENCIA_PAGO VARCHAR(100) NULL, -- Número de comprobante o voucher
    OBSERVACION VARCHAR(255) NULL,     -- Notas del gestor
    
    -- Estado del Cobro (Para los filtros y semáforos del dashboard)
    -- Valores sugeridos: 'PENDIENTE', 'PAGADO', 'PARCIAL', 'ANULADO'
    ESTADO_COBRANZA VARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',

    -- Auditoría (Estándar de tu sistema)
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    USRCreacion VARCHAR(50) NOT NULL DEFAULT 'SYSTEM',
    FechaActualizacion DATETIME NULL,
    USRActualizacion VARCHAR(50) NULL,
    UsuarioIP VARCHAR(50) NULL,
    Estado BIT NOT NULL DEFAULT 1,

    -- Llave foránea
    FOREIGN KEY (IDUSRSEGUROSFK) REFERENCES USRASEGURADOS(IDUSRSEGUROS)
);


CREATE TRIGGER TRG_GENERAR_COBRANZA
ON USRASEGURADOS
AFTER INSERT
AS
BEGIN
    INSERT INTO COBRANZAS (
        IDUSRSEGUROSFK, 
        FECHA_VENCIMIENTO, 
        MONTO_ESPERADO, 
        USRCreacion
    )
    SELECT 
        i.IDUSRSEGUROS,
        DATEADD(YEAR, 1, i.FECHACONTRATASEGURO), -- Ejemplo: Vence en 1 año
        s.PRIMA, -- Sacamos el precio de la tabla SEGUROS
        i.USRCreacion
    FROM inserted i
    INNER JOIN SEGUROS s ON i.CODSEGUROFK = s.CODSEGURO
END;


SELECT 
    C.IDCOBRANZA,
    A.NMBRCOMPLETO AS CLIENTE,
    S.NMBRSEGURO AS POLIZA,
    C.FECHA_VENCIMIENTO,
    C.MONTO_ESPERADO,
    C.ESTADO_COBRANZA,
    -- Columna calculada para saber si hay MORA
    CASE 
        WHEN C.ESTADO_COBRANZA = 'PENDIENTE' AND C.FECHA_VENCIMIENTO < GETDATE() THEN 'MORA'
        WHEN C.ESTADO_COBRANZA = 'PENDIENTE' AND C.FECHA_VENCIMIENTO >= GETDATE() THEN 'AL DIA'
        ELSE 'PAGADO'
    END AS ESTADO_CALCULADO,
    -- Días de retraso
    DATEDIFF(day, C.FECHA_VENCIMIENTO, GETDATE()) AS DIAS_RETRASO
FROM COBRANZAS C
INNER JOIN USRASEGURADOS UA ON C.IDUSRSEGUROSFK = UA.IDUSRSEGUROS
INNER JOIN ASEGURADOS A ON UA.CEDULAFK = A.CEDULA
INNER JOIN SEGUROS S ON UA.CODSEGUROFK = S.CODSEGURO
WHERE C.Estado = 1;