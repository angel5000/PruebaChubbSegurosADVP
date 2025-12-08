USE chubbseguros;--usa la base datos

--CREACION DE SP-----
EXEC sp_helptext 'REGASEGURAMIENTO'
exec CONSULTASEGUROS
CREATE OR ALTER PROCEDURE CONSULTASEGUROS
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



CREATE OR ALTER PROCEDURE CONSULSEGID
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
        EDADMAX,
		FechaCreacion,
        USRCreacion,
        FechaActualizacion,
        USRActualizacion,
		UsuarioIP,
		Estado
		FROM SEGUROS
		WHERE IDSEGURO = @IDSEGURO
	END;
GO


------------
-------------

CREATE OR ALTER PROCEDURE CONSULTAASEGURADOS
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
CREATE OR ALTER PROCEDURE REGITSSEGUROS(
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
    RAISERROR (
        'El código del seguro ya existe. Código duplicado: %s',
        16,
        1,
        @CODSEGURO
    );
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


CREATE OR ALTER PROCEDURE EDITARSEGUROS
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

CREATE OR ALTER PROCEDURE ELIMINARSEGURO(
    @IDSEGURO INT,
    @USRActualizacion VARCHAR(50),
    @UsuarioIP VARCHAR(50),
    @EstadoDT VARCHAR(15)
)
AS 
BEGIN
    SET NOCOUNT ON;

    -- 1. Validar que el seguro exista y obtener su CODIGO para verificar relaciones
    DECLARE @CodSeguro VARCHAR(10);

    SELECT @CodSeguro = CODSEGURO 
    FROM SEGUROS 
    WHERE IDSEGURO = @IDSEGURO;

    IF @CodSeguro IS NULL
    BEGIN
        RAISERROR('Seguro no existe para eliminar', 16, 1);
        RETURN -1; -- Registro no encontrado
    END

    IF EXISTS (SELECT 1 FROM USRASEGURADOS WHERE CODSEGUROFK = @CodSeguro AND Estado = 1)
    BEGIN
        -- Si existe al menos uno, lanzamos error y detenemos el proceso
        RAISERROR('No se puede eliminar el seguro porque tiene asegurados activos asociados.', 16, 1);
        RETURN -2; -- Código de error por dependencia
    END

    -- Inicio de Transacción para asegurar integridad (Histórico + Borrado)
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 3. Guardar en Histórico
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

        -- 4. Eliminar el registro
        DELETE FROM SEGUROS
        WHERE IDSEGURO = @IDSEGURO;

        COMMIT TRANSACTION;
        RETURN 1; -- Éxito
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -99;
    END CATCH

END;

------------------------------------
		--ASEGURADOS--
------------------------------------

CREATE OR ALTER PROCEDURE CONSULASGURADOSEGID
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


CREATE OR ALTER PROCEDURE REGSTASEGURADOS(
    @CEDULA        VARCHAR(10),
    @NMBRCOMPLETO  VARCHAR(75),
    @TELEFONO      VARCHAR(10),
    @EDAD          INT
)
AS
BEGIN
  SET NOCOUNT ON;

IF EXISTS (SELECT 1 FROM ASEGURADOS  WHERE CEDULA = @CEDULA)
BEGIN
    DECLARE @NOMBRECORTO VARCHAR(50);

    -- Tomar solo los primeros 20 caracteres del nombre completo
    SET @NOMBRECORTO = SUBSTRING(@NMBRCOMPLETO, 1, 14);

    RAISERROR(
        'El Usuario %s... con cédula %s ya se encuentra registrado',
        16,
        1,
        @NOMBRECORTO,
        @CEDULA
    );
    
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

CREATE OR ALTER PROCEDURE EDITARASEGURADOS
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


CREATE OR ALTER PROCEDURE ELIMINARASEGURADO(
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


CREATE OR ALTER PROCEDURE CONSULTASEGURAMIENTO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        -- Datos base
        A.IDASEGURADOS,
        A.CEDULA,
        A.NMBRCOMPLETO,
        A.EDAD,

        -- ID aseguramiento
        CASE 
            WHEN U.IDUSRSEGUROS IS NULL THEN 0
            ELSE U.IDUSRSEGUROS
        END AS IDUSRSEGUROS,

        -- Seguro
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
        END AS FECHACONTRATASEGURO,

        -- ===============================
        -- CAMPOS DE AUDITORÍA USRASEGURADOS
        -- ===============================
        U.FechaCreacion AS FechaCreacion,
        U.USRCreacion AS USRCreacion,
        U.FechaActualizacion AS FechaActualizacion,
        U.USRActualizacion AS USRActualizacion,
        U.UsuarioIP AS UsuarioIP,
        U.Estado AS Estado


    FROM 
        ASEGURADOS A
        LEFT JOIN USRASEGURADOS U ON A.CEDULA = U.CEDULAFK
        LEFT JOIN SEGUROS S ON S.CODSEGURO = U.CODSEGUROFK;

END;
GO



CREATE OR ALTER PROCEDURE REGASEGURAMIENTO(  
  @CEDULA VARCHAR(10),  
  @CODSEGURO VARCHAR(10)   
)  
AS  
BEGIN  
    SET NOCOUNT ON;  

    DECLARE 
        @NOMBREASEG VARCHAR(75),
        @NOMBREASEG_SUB VARCHAR(15),
        @NOMBRESEGURO VARCHAR(75),
        @NOMBRESEGURO_SUB VARCHAR(15),
        @EDADASEG INT,
        @EDADMIN INT,
        @EDADMAX INT;

    ------------------------------------------
    -- Validar asegurado
    ------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM ASEGURADOS WHERE CEDULA = @CEDULA)  
    BEGIN  
        RAISERROR('El asegurado con cédula %s no existe.', 16, 1, @CEDULA);  
        RETURN -1;  
    END  

    SELECT 
        @NOMBREASEG = NMBRCOMPLETO,
        @EDADASEG   = EDAD
    FROM ASEGURADOS 
    WHERE CEDULA = @CEDULA;

    -- Substring del nombre
    SET @NOMBREASEG_SUB = SUBSTRING(@NOMBREASEG, 1, 15);

    ------------------------------------------
    -- Validar seguro
    ------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM SEGUROS WHERE CODSEGURO = @CODSEGURO)  
    BEGIN  
        RAISERROR('El seguro con código %s no existe.', 16, 1, @CODSEGURO);  
        RETURN -2;  
    END  

    SELECT
        @NOMBRESEGURO = NMBRSEGURO,
        @EDADMIN      = EDADMIN,
        @EDADMAX      = EDADMAX
    FROM SEGUROS
    WHERE CODSEGURO = @CODSEGURO;

    -- Substring del nombre del seguro
    SET @NOMBRESEGURO_SUB = SUBSTRING(@NOMBRESEGURO, 1, 15);

    ------------------------------------------
    -- Validar rango de edad
    ------------------------------------------
    IF @EDADASEG < @EDADMIN OR @EDADASEG > @EDADMAX
    BEGIN
        RAISERROR(
            'El asegurado %s no cumple la edad requerida para el seguro %s.',
            16, 1,
            @NOMBREASEG_SUB,
            @NOMBRESEGURO_SUB  
        );
        RETURN -4;
    END

    ------------------------------------------
    -- Validar seguro ya asignado
    ------------------------------------------
    IF EXISTS (
        SELECT 1
        FROM USRASEGURADOS
        WHERE CEDULAFK = @CEDULA
          AND CODSEGUROFK = @CODSEGURO
    )
    BEGIN
        RAISERROR(
            'El asegurado %s ya tiene registrado el seguro %s.',
            16, 1,
            @NOMBREASEG_SUB,
            @NOMBRESEGURO_SUB
        );  
        RETURN -3;  
    END  

    ------------------------------------------
    -- Registrar seguro
    ------------------------------------------
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


CREATE OR ALTER PROCEDURE SEGUR0SDISPO
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
	    IDSEGURO,
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


CREATE OR ALTER PROCEDURE ConsulAseguradosPorSeguros
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
          CONVERT(VARCHAR(10), U.FECHACONTRATASEGURO, 120) AS FECHACONTRATASEGURO
			
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


----------------------------------------
------------LOGIN---------------
----------------------------------------

CREATE OR ALTER PROCEDURE LoginUsuario
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

    -- Validar contraseña → HASH(SALT + CONTRASEÑAINGRESADA)
    DECLARE @HashIngresado VARBINARY(64);
    SET @HashIngresado = HASHBYTES('SHA2_512', @PasswordSalt + CONVERT(VARBINARY(200), @Contraseña));

    IF @HashIngresado <> @PasswordHash
    BEGIN
        RAISERROR('Contraseña incorrecta.', 16, 1);
        RETURN -2;
    END

    -- Obtener Rol principal del usuario
    SELECT TOP 1
        @IdRol = R.IdRol,
        @NombreRol = R.NombreRol
    FROM UsuarioRoles UR
        INNER JOIN Roles R ON UR.IdRol = R.IdRol
    WHERE UR.IdUsuario = @IdUsuario
    ORDER BY R.IdRol;  -- opcional: primer rol asignado

    -- Retornar los datos completos
    SELECT 
        @IdUsuario     AS IdUsuario,
        @NombreUsuario AS NombreUsuario,
        @Correo        AS Correo,
        @IdRol         AS IdRol,
        @NombreRol     AS NombreRol,
        @Estado        AS Estado;

    RETURN 1; 
END;
GO

DECLARE @Resultado INT;

EXEC @Resultado = LoginUsuario 
    @Credenciales = 'admin@aseguradora.com',
    @Contraseña = '123456';

SELECT @Resultado AS Resultado;


CREATE OR ALTER PROCEDURE COBRANZASTOTALES

AS
BEGIN
    SET NOCOUNT ON;
	SELECT 

    C.IDCOBRANZA,
	UA.IDUSRSEGUROS AS IDASEGURADO,
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

	END;
GO

---------------
---------------

CREATE OR ALTER PROCEDURE CONSULTARPERMISOS
(
    @IDROL  INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       IdRol,
	   IdPermiso,
	   Estado,
	   FechaAsignacion
    FROM RolesPermisos
	WHERE IdRol = @IDROL
END;
GO


CREATE OR ALTER PROCEDURE CANCELAR_POLIZA
    @IDUSRSEGUROS INT,
    @USUARIO_ANULA VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Desactivar la Póliza (Soft Delete)
        UPDATE USRASEGURADOS
        SET Estado = 0, -- 0 = Inactivo
            FechaActualizacion = GETDATE(),
            USRActualizacion = @USUARIO_ANULA
        WHERE IDUSRSEGUROS = @IDUSRSEGUROS;

        -- Anular SOLO los cobros pendientes (No tocar lo pagado)
        UPDATE COBRANZAS
        SET ESTADO_COBRANZA = 'ANULADO',
            OBSERVACION = 'Póliza Cancelada por el usuario',
            FechaActualizacion = GETDATE(),
            USRActualizacion = @USUARIO_ANULA
        WHERE IDUSRSEGUROSFK = @IDUSRSEGUROS 
          AND ESTADO_COBRANZA = 'PENDIENTE'; 
	
        COMMIT TRANSACTION;
			   RETURN 1;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Relanzar el error para que Angular lo detecte
    END CATCH
END;


CREATE OR ALTER PROCEDURE ELIMINARASEGURAMIENTO
    @IDUSRSEGUROS INT,
    @USUARIO_EJECUTA VARCHAR(50) 
AS 
BEGIN
    SET NOCOUNT ON;

    -- Variables para leer el estado antes de borrar
    DECLARE @EstadoActual BIT;
    DECLARE @Cedula VARCHAR(10);
    DECLARE @CodSeguro VARCHAR(10);
    DECLARE @FechaContrata DATE;

    -- Verificar si existe y obtener datos
    SELECT 
        @EstadoActual = Estado,
        @Cedula = CEDULAFK,
        @CodSeguro = CODSEGUROFK,
        @FechaContrata = FECHACONTRATASEGURO
    FROM USRASEGURADOS 
    WHERE IDUSRSEGUROS = @IDUSRSEGUROS;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('El registro de aseguramiento no existe.', 16, 1);
        RETURN -1;
    END

    -- Verificar Estado (Solo si es 0 se elimina)
    IF @EstadoActual = 1
    BEGIN
        RAISERROR('No se puede eliminar un seguro activo', 16, 1);
        RETURN -2;
    END

    -- INICIO DE TRANSACCIÓN (Todo o Nada)
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 3. Guardar en Histórico (Backup antes de morir)
        INSERT INTO USRASEGURADOS_HISTORICO (
            IDUSRSEGUROS_ORIGINAL, CEDULAFK, CODSEGUROFK, FECHACONTRATASEGURO, UsuarioElimino
        )
        VALUES (
            @IDUSRSEGUROS, @Cedula, @CodSeguro, @FechaContrata, @USUARIO_EJECUTA
        );

        -- Eliminar dependencias en COBRANZAS (Hijos)
        -- Borramos todo rastro financiero de este seguro específico
        DELETE FROM COBRANZAS 
        WHERE IDUSRSEGUROSFK = @IDUSRSEGUROS;

        -- Eliminar el registro principal en USRASEGURADOS (Padre)
        DELETE FROM USRASEGURADOS
        WHERE IDUSRSEGUROS = @IDUSRSEGUROS;

        -- Si todo salió bien, confirmamos
        COMMIT TRANSACTION;
        RETURN 1; -- Éxito

    END TRY
    BEGIN CATCH
        -- Si algo falla, revertimos todo (no se borra nada ni se inserta histórico)
        ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -99;
    END CATCH
END;

