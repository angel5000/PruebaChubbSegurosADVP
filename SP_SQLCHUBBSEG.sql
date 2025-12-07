USE chubbseguros;--usa la base datos

--CREACION DE SP-----

exec CONSULTASEGUROS
CREATE PROCEDURE CONSULTASEGUROS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDSEGURO,
        NMBRSEGURO,
        CODSEGURO,
        SUMASEGURADA,
        PRIMA,
        EDADMIN,
        EDADMAX,
        FechaCreacion,
        USRCreacion,
        FechaActualizacion,
        USRActualizacion,
		UsuarioIP,
		Estado
    FROM SEGUROS;
END;
GO



CREATE PROCEDURE CONSULSEGID
(
    @IDSEGURO  INT
)
AS
BEGIN
 SET NOCOUNT ON;
 SELECT
		IDSEGURO,
        NMBRSEGURO,
        CODSEGURO,
        SUMASEGURADA,
        PRIMA,
        EDADMIN,
        EDADMAX
		FROM SEGUROS
		WHERE IDSEGURO = @IDSEGURO
	END;
GO


------------
-------------

CREATE PROCEDURE CONSULTAASEGURADOS
AS
BEGIN
 SET NOCOUNT ON;
 SELECT
	IDASEGURADOS,
    CEDULA,
    NMBRCOMPLETO,
    TELEFONO,
    EDAD
    FROM ASEGURADOS
	END;
GO
EXEC CONSULTAASEGURADOS;

--------------
----------------
CREATE PROCEDURE REGITSSEGUROS(
    @NMBRSEGURO        VARCHAR(75),
    @CODSEGURO         VARCHAR(10),
    @SUMASEGURADA      DECIMAL(10,2),
    @PRIMA             DECIMAL(10,2),
    @EDADMIN           INT,
    @EDADMAX           INT,
    @USRCreacion          VARCHAR(50),
    @UsuarioIP            VARCHAR(50),
    @Estado               INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- VALIDACIÓN: Código de seguro duplicado
    IF EXISTS (SELECT 1 FROM SEGUROS WHERE CODSEGURO = @CODSEGURO)
    BEGIN
        RAISERROR ('El código del seguro ya existe.', 16, 1);
        RETURN -1; 
    END

    -- INSERT
    INSERT INTO SEGUROS
    (
        NMBRSEGURO,
        CODSEGURO,
        SUMASEGURADA,
        PRIMA,
        EDADMIN,
        EDADMAX,
        FechaCreacion,
        USRCreacion,
        FechaActualizacion,
        USRActualizacion,
        UsuarioIP,
        Estado
    )
    VALUES
    (
        @NMBRSEGURO,
        @CODSEGURO,
        @SUMASEGURADA,
        @PRIMA,
        @EDADMIN,
        @EDADMAX,
        GETDATE(),
        @USRCreacion,
        NULL,
        '',
        @UsuarioIP,
        @Estado
    );

    RETURN 1; 
END;
GO



-----------------------------------
-----------------------------------


CREATE PROCEDURE EDITARSEGUROS
(
    @IDSEGURO       INT,
    @NMBRSEGURO     VARCHAR(75),
    @CODSEGURO      VARCHAR(10),
    @SUMASEGURADA   DECIMAL(10,2),
    @PRIMA          DECIMAL(10,2),
    @EDADMIN        INT,
    @EDADMAX        INT,
	@USRActualizacion   VARCHAR(50),
    @UsuarioIP      VARCHAR(50),
    @Estado         INT
)
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM SEGUROS WHERE IDSEGURO =@IDSEGURO)
  BEGIN
     RAISERROR('El seguro especificado no existe.', 16, 1);
        RETURN -1;  -- ID no encontrado
    END
	IF EXISTS (SELECT 1 FROM SEGUROS WHERE CODSEGURO =@CODSEGURO AND IDSEGURO <> @IDSEGURO)
	BEGIN
	RAISERROR('El código del seguro ya está asignado a otro registro.', 16, 1);
        RETURN -2;  -- duplicado
    END

	UPDATE SEGUROS
	SET
	  NMBRSEGURO   = @NMBRSEGURO,
        CODSEGURO    = @CODSEGURO,
        SUMASEGURADA = @SUMASEGURADA,
        PRIMA        = @PRIMA,
        EDADMIN      = @EDADMIN,
        EDADMAX      = @EDADMAX,
		USRActualizacion=@USRActualizacion,
		FechaActualizacion=GETDATE(),
		UsuarioIP=@UsuarioIP,
		Estado =@Estado
    WHERE IDSEGURO = @IDSEGURO;
	RETURN 1; -- éxito
END;


---------------------------
-------------------------------

CREATE PROCEDURE ELIMINARSEGURO(
		@IDSEGURO INT,
		@USRActualizacion   VARCHAR(50),
        @UsuarioIP      VARCHAR(50),
        @EstadoDT         VARCHAR(15)
)
AS 
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM SEGUROS WHERE IDSEGURO =@IDSEGURO)
	BEGIN
	 RAISERROR('Seguro no existe para eliminar', 16, 1);
        RETURN -1;   -- Registro no encontrado
    END
	  INSERT INTO SEGUROS_HISTORICO (
        IDSEGURO,
        NMBRSEGURO,
        CODSEGURO,
        SUMASEGURADA,
        PRIMA,
        EDADMIN,
        EDADMAX,
        EstadoDT,
        FechaModElim,
        UsuarioIP,
        USRActualizacion
    )
    SELECT 
        IDSEGURO,
        NMBRSEGURO,
        CODSEGURO,
        SUMASEGURADA,
        PRIMA,
        EDADMIN,
        EDADMAX,
        @EstadoDT,
        GETDATE(),   
		@UsuarioIP,
        @USRActualizacion
    FROM SEGUROS
    WHERE IDSEGURO = @IDSEGURO;

	DELETE FROM SEGUROS
	WHERE IDSEGURO =@IDSEGURO;
	
	RETURN 1;

END;

------------------------------------
		--ASEGURADOS--
------------------------------------

CREATE PROCEDURE CONSULASGURADOSEGID
(
    @IDASEGURADOS  INT
)
AS
BEGIN
 SET NOCOUNT ON;
 SELECT
		IDASEGURADOS,
		CEDULA,
        NMBRCOMPLETO,
        TELEFONO,
        EDAD
		FROM ASEGURADOS
		WHERE IDASEGURADOS = @IDASEGURADOS
	END;
GO


CREATE PROCEDURE REGSTASEGURADOS(
    @CEDULA        VARCHAR(10),
    @NMBRCOMPLETO  VARCHAR(75),
    @TELEFONO      VARCHAR(10),
    @EDAD          INT
)
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM ASEGURADOS WHERE CEDULA = @CEDULA)
  BEGIN
   RAISERROR('Usuario ya se encuentra registrado', 16, 1);
        RETURN -1;   -- Cedula duplicada
    END

	INSERT INTO ASEGURADOS
	(
	    CEDULA,
        NMBRCOMPLETO,
        TELEFONO,
        EDAD
    )
    VALUES
    (
        @CEDULA,
        @NMBRCOMPLETO,
        @TELEFONO,
        @EDAD
    );
	RETURN 1;
END;

CREATE PROCEDURE EDITARASEGURADOS
(
    @IDASEGURADOS       INT,
    @CEDULA        VARCHAR(10),
    @NMBRCOMPLETO  VARCHAR(75),
    @TELEFONO      VARCHAR(10),
    @EDAD          INT
)
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM ASEGURADOS WHERE IDASEGURADOS =@IDASEGURADOS)
  BEGIN
     RAISERROR('El cliente a modificar no existe', 16, 1);
        RETURN -1;  -- ID no encontrado
    END
	IF EXISTS (SELECT 1 FROM ASEGURADOS WHERE CEDULA =@CEDULA AND IDASEGURADOS <> @IDASEGURADOS)
	BEGIN
	RAISERROR('Cedula ingresada ya esta registrada en el sistema', 16, 1);
        RETURN -2;  -- duplicado
    END

	UPDATE ASEGURADOS
	SET
		CEDULA        =  @CEDULA,
		NMBRCOMPLETO  =  @NMBRCOMPLETO,
		TELEFONO      =	 @TELEFONO,
		EDAD		  =	 @EDAD
	
    WHERE IDASEGURADOS = @IDASEGURADOS;
	RETURN 1; -- éxito
END;


CREATE PROCEDURE ELIMINARASEGURADO(
		@IDASEGURADOS INT
)
AS 
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM ASEGURADOS WHERE IDASEGURADOS =@IDASEGURADOS)
	BEGIN
	 RAISERROR('Cliente no existe para eliminar', 16, 1);
        RETURN -1;   -- Registro no encontrado
    END
	DELETE FROM ASEGURADOS
	WHERE IDASEGURADOS =@IDASEGURADOS;
	
	RETURN 1;

END;
-------------------------
------ASEGURAMIENTOS-------
----------------------------

exec CONSULTASEGURAMIENTO
CREATE PROCEDURE CONSULTASEGURAMIENTO
AS
BEGIN
    SET NOCOUNT ON;

     SELECT 
        A.IDASEGURADOS,
        A.CEDULA,
        A.NMBRCOMPLETO,
        A.EDAD,
		 CASE 
            WHEN U.IDUSRSEGUROS IS NULL THEN 0
            ELSE U.IDUSRSEGUROS
        END AS IDUSRSEGUROS,
    CASE 
            WHEN S.NMBRSEGURO IS NULL THEN 'SIN SEGURO ASIGNADO'
            ELSE S.NMBRSEGURO
        END AS NMBRSEGURO,

        CASE 
            WHEN S.CODSEGURO IS NULL THEN '0'
            ELSE S.CODSEGURO
        END AS CODSEGURO,

        CASE 
            WHEN S.SUMASEGURADA IS NULL THEN 0
            ELSE S.SUMASEGURADA
        END AS SUMASEGURADA,

        CASE 
            WHEN S.PRIMA IS NULL THEN 0
            ELSE S.PRIMA
        END AS PRIMA,

       CASE 
			WHEN U.FECHACONTRATASEGURO IS NULL THEN 'N/A'
			ELSE CONVERT(VARCHAR(10), U.FECHACONTRATASEGURO, 120)
			END AS FECHACONTRATASEGURO


    FROM 
      ASEGURADOS A
        LEFT JOIN USRASEGURADOS U ON A.CEDULA = U.CEDULAFK
        LEFT JOIN SEGUROS S ON S.CODSEGURO = U.CODSEGUROFK;;
   
END;
GO


CREATE PROCEDURE REGASEGURAMIENTO(
		@CEDULA VARCHAR(10),
		@CODSEGURO VARCHAR(10) 
)
AS
BEGIN
   SET NOCOUNT ON;
   IF NOT EXISTS (SELECT 1 FROM ASEGURADOS WHERE CEDULA =@CEDULA)
   BEGIN
   RAISERROR('El asegurado no existe.', 16, 1);
        RETURN -1;
    END

    -- Validar que el seguro exista
    IF NOT EXISTS (SELECT 1 FROM SEGUROS WHERE CODSEGURO = @CODSEGURO)
    BEGIN
        RAISERROR('El seguro no existe.', 16, 1);
        RETURN -2;
    END

	 IF EXISTS (
        SELECT 1 
        FROM USRASEGURADOS 
        WHERE CEDULAFK = @CEDULA 
          AND CODSEGUROFK = @CODSEGURO
    )
    BEGIN
        RAISERROR('El cliente ya tiene registrado este seguro.', 16, 1);
        RETURN -3;
    END

	INSERT INTO USRASEGURADOS (
        CEDULAFK,
        CODSEGUROFK,
        FECHACONTRATASEGURO
    )
    VALUES (
        @CEDULA,
        @CODSEGURO,
        GETDATE()
    );
	RETURN 1; 
END;


CREATE PROCEDURE SEGUR0SDISPO
    @EDAD INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM SEGUROS WHERE @EDAD BETWEEN EDADMIN AND EDADMAX
    )
    BEGIN
        SELECT 
            'SIN_DATOS' AS CODSEGURO,
            'No hay seguros disponibles' AS NMBRSEGURO,
            0 AS EDADMIN,
            0 AS EDADMAX,
            CAST(0 AS DECIMAL(18,2)) AS SUMASEGURADA,
			CAST(0 AS DECIMAL(18,2)) AS PRIMA;
        RETURN;
    END

    SELECT 
        CODSEGURO,
        NMBRSEGURO,
        EDADMIN,
        EDADMAX,
        SUMASEGURADA,
        PRIMA
    FROM SEGUROS
    WHERE @EDAD BETWEEN EDADMIN AND EDADMAX;
END
GO

CREATE PROCEDURE ELIMINARASEGURAMIENTO(
		@IDUSRSEGUROS INT
)
AS 
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM USRASEGURADOS WHERE IDUSRSEGUROS = @IDUSRSEGUROS)
	BEGIN
	 RAISERROR('Aseguramiento no existe para eliminar', 16, 1);
        RETURN -1;   -- Registro no encontrado
    END
	DELETE FROM USRASEGURADOS
	WHERE IDUSRSEGUROS = @IDUSRSEGUROS;
	
	RETURN 1;

END;



CREATE PROCEDURE ConsulAseguradosPorSeguros
    @IDSEGURO INT = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    -- Si no se envía IDSEGURO, mostrar cantidad de asegurados por seguro
    IF @IDSEGURO IS NULL
    BEGIN
        SELECT 
            s.IDSEGURO,
            s.NMBRSEGURO,
            s.CODSEGURO,
            COUNT(u.CEDULAFK) AS CantidadAsegurados
        FROM SEGUROS s
        LEFT JOIN USRASEGURADOS u
            ON s.CODSEGURO = u.CODSEGUROFK
        WHERE s.Estado = 1
        GROUP BY s.IDSEGURO, s.NMBRSEGURO, s.CODSEGURO
        ORDER BY s.NMBRSEGURO;
    END
    ELSE
    BEGIN
        -- Mostrar la lista de asegurados de un seguro específico
        SELECT 
            a.IDASEGURADOS,
            a.CEDULA,
            a.NMBRCOMPLETO,
            a.TELEFONO,
            a.EDAD,
            u.FECHACONTRATASEGURO
        FROM ASEGURADOS a
        INNER JOIN USRASEGURADOS u
            ON a.CEDULA = u.CEDULAFK
        INNER JOIN SEGUROS s
            ON s.CODSEGURO = u.CODSEGUROFK
        WHERE s.IDSEGURO = @IDSEGURO
            AND a.Estado = 1
            AND s.Estado = 1
            AND u.Estado = 1
        ORDER BY a.NMBRCOMPLETO;
    END
END
GO

EXEC ConsulAseguradosPorSeguros @IDSEGURO = 5;






----------------------------------------
------------LOGIN---------------
----------------------------------------

CREATE PROCEDURE LoginUsuario
    @Credenciales       VARCHAR(100), 
    @Contraseña         NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @IdUsuario INT,
        @NombreUsuario VARCHAR(50),
        @Correo VARCHAR(100),
        @PasswordHash VARBINARY(64),
        @PasswordSalt VARBINARY(64),
        @Estado INT,
        @IdRol INT,
        @NombreRol VARCHAR(100);

    -- 1️⃣ Obtener datos del usuario
    SELECT TOP 1
        @IdUsuario = U.IdUsuario,
        @NombreUsuario = U.NombreUsuario,
        @Correo = U.Correo,
        @PasswordHash = U.PasswordHash,
        @PasswordSalt = U.PasswordSalt,
        @Estado = U.Estado
    FROM Usuarios U
    WHERE (U.NombreUsuario = @Credenciales OR U.Correo = @Credenciales);

    -- 2️⃣ Validar existencia
    IF @IdUsuario IS NULL
    BEGIN
        RAISERROR('El usuario o correo no existe.', 16, 1);
        RETURN -1;
    END

    -- 3️⃣ Validar estado activo
    IF @Estado <> 1
    BEGIN
        RAISERROR('El usuario está inactivo.', 16, 1);
        RETURN -3;
    END

    -- 4️⃣ Validar contraseña → HASH(SALT + CONTRASEÑAINGRESADA)
    DECLARE @HashIngresado VARBINARY(64);
    SET @HashIngresado = HASHBYTES('SHA2_512', @PasswordSalt + CONVERT(VARBINARY(200), @Contraseña));

    IF @HashIngresado <> @PasswordHash
    BEGIN
        RAISERROR('Contraseña incorrecta.', 16, 1);
        RETURN -2;
    END

    -- 5️⃣ Obtener Rol principal del usuario
    SELECT TOP 1
        @IdRol = R.IdRol,
        @NombreRol = R.NombreRol
    FROM UsuarioRoles UR
        INNER JOIN Roles R ON UR.IdRol = R.IdRol
    WHERE UR.IdUsuario = @IdUsuario
    ORDER BY R.IdRol;  -- opcional: primer rol asignado

    -- 6️⃣ Retornar los datos completos
    SELECT 
        @IdUsuario     AS IdUsuario,
        @NombreUsuario AS NombreUsuario,
        @Correo        AS Correo,
        @IdRol         AS IdRol,
        @NombreRol     AS NombreRol,
        @Estado        AS Estado;

    RETURN 1; -- login exitoso
END;
GO


DECLARE @Resultado INT;

EXEC @Resultado = LoginUsuario 
    @Credenciales = 'admin@aseguradora.com',
    @Contraseña = '123456';

SELECT @Resultado AS Resultado;
