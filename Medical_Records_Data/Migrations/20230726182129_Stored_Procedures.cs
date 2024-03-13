using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medical_Record_Data.Migrations
{
    /// <inheritdoc />
    public partial class Stored_Procedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //==========================================
            // ============= AGREGAR USUARIO ===========
            //==========================================
            migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[SP_P_AgregarUsuario]
                -- Add the parameters for the stored procedure here
               @Usuario VARCHAR(70) , --PK
               @Nombre VARCHAR(100) ,
               @Tipo VARCHAR(2) ,
               @Activo BIT,   
               @Contrasena VARCHAR(100),
               @CorreoElectronico VARCHAR(100),   
               @Resultado BIT OUTPUT
              AS
              BEGIN
                SET NOCOUNT OFF;

                  IF (NOT EXISTS(SELECT * FROM USUARIO WHERE NombreUsuario = @Usuario))
                  BEGIN
                  DECLARE @pass VARCHAR(100)
                  SELECT @pass = dbo.F_Encrypt(@Contrasena)

                      INSERT INTO USUARIO
                      VALUES(@Usuario, @Nombre, @Tipo, @Activo,(SELECT @pass), @CorreoElectronico)
                      SET @Resultado = 1
                  
                  END
                  ELSE
                  BEGIN
                      SET @Resultado = 0
                 
                END
              END
            GO

            ");
            //==========================================
            // ============= MOSTRAR USUARIOS ==========
            //==========================================
            migrationBuilder.Sql(@"
                
                CREATE PROCEDURE [dbo].[SP_C_Mostrar_Usuarios]
                  @NPag int, --Numero de pagina que va ha mostrar 
                  @CantReg int, -- Cantidad de registro a mostrar proveniente del select de la vista
                  @palabraBusc varchar(100),
                  -- PTR SALIDA
                  @TotalPag INT output -- retorna la cantidad de paginas existentes
                  AS 
                  BEGIN

                    DECLARE @residuo DECIMAL(20,1)
                    DECLARE @skipReg int = @NPag * @CantReg -- 
                    --Cantidad de registros existentes
                    DECLARE @cantRegistExist DECIMAL(20,1)  = (SELECT count(u.NombreUsuario)
                                          FROM USUARIO AS u
                                          WHERE
                                            u.NombreUsuario like '%'+@palabraBusc+'%' OR
                                            u.Nombre like '%'+@palabraBusc+'%' OR
                                            u.Tipo like '%'+@palabraBusc+'%'OR												  												  
                                            u.Contrasena like '%'+@palabraBusc+'%'OR
                                            u.CorreoElectronico like '%'+@palabraBusc+'%')
                            
    
                    SET @residuo = @cantRegistExist % @CantReg
                    SET @cantRegistExist = @cantRegistExist / @CantReg
                    SET @TotalPag = @cantRegistExist

                    IF(@residuo > 0)-- agregamos una pagina mas si es impar
                    BEGIN
                      SET @TotalPag += 1
                    END
    
                    -- ESTO ES LO QUE RETORNA

                      (SELECT *
                      FROM USUARIO AS u
                      WHERE
                        u.NombreUsuario like '%'+@palabraBusc+'%' OR
                        u.Nombre like '%'+@palabraBusc+'%' OR
                        u.Tipo like '%'+@palabraBusc+'%'OR
                        u.Activo like '%'+@palabraBusc+'%'OR
                        u.Contrasena like '%'+@palabraBusc+'%'OR			
                        u.CorreoElectronico like '%'+@palabraBusc+'%'
                        )
                      ORDER BY u.NombreUsuario
                      OFFSET @skipReg ROWS
                      FETCH NEXT @CantReg ROWS ONLY
                  END
                GO

            ");
            //==========================================
            // ============= EDITAR USUARIO ===========
            //==========================================
            migrationBuilder.Sql(@"
        
                CREATE PROCEDURE [dbo].[SP_P_Modificar_Usuario]
	                --Parámetros necesarios para el procedimiento
	                @Usuario VARCHAR(70),--PK
	                @Nombre VARCHAR(100),
	                @Tipo VARCHAR(2),
	                @Activo BIT,	
	                @Contrasena VARCHAR(100) NULL,
	                @CorreoElectronico VARCHAR(100),	
	                @Resultado BIT OUTPUT,
	                @Mensaje VARCHAR(500) OUTPUT

                    AS
                    BEGIN

                        SET NOCOUNT ON;

                        IF(EXISTS(SELECT * FROM USUARIO WHERE NombreUsuario = @Usuario))
                        BEGIN
                            DECLARE @pass VARCHAR(100)
                            SELECT @pass = dbo.F_Encrypt(@Contrasena)

                            UPDATE USUARIO 
                            SET Nombre = @Nombre, 
                                Tipo = @Tipo, 
                                Activo = @Activo,			 
                                CorreoElectronico = @CorreoElectronico
                
                            WHERE NombreUSUARIO = @Usuario

                            IF(@Contrasena is not null)
                            BEGIN
                                Update USUARIO
                                SET Contrasena = (SELECT dbo.F_Encrypt(@Contrasena))
                    
                                WHERE NombreUsuario = @Usuario
                            END

                            SET @Resultado = 1
                            SET @Mensaje = 'Usuario modificado exitosamente'
                        END
                        ELSE
                        BEGIN
                            SET @Resultado = 0
                            SET @Mensaje = 'Error al modificar el usuario'
                        END
                    END

                GO
            ");
            //==========================================
            // ============= DES/ACTIVAR USUARIO ===========
            //==========================================
            migrationBuilder.Sql(@"
             CREATE PROCEDURE [dbo].[SP_P_Modificar_Estado_Usuario]
                 @Usuario varchar(70),
                 @Resultado BIT OUTPUT,
                 @Mensaje VARCHAR(500) OUTPUT
                 AS
	                DECLARE @Estado bit
                 BEGIN
	                IF(EXISTS (SELECT NombreUsuario FROM USUARIO WHERE NombreUsuario = @Usuario))
		                BEGIN
			                IF(EXISTS (SELECT Activo FROM USUARIO WHERE Activo = 1 AND NombreUsuario = @Usuario))
				                BEGIN 
					                SET @Estado = 0
				                END
			                ELSE
				                BEGIN 
					                SET @Estado = 1
				                END

				                UPDATE USUARIO SET Activo = @Estado	WHERE NombreUsuario = @Usuario
				                SET @Resultado = 1
				                SET @Mensaje = 'Estado cambiado correctamente'
		                END
	                ELSE
		                BEGIN 
			                SET @Resultado = 0
				                SET @Mensaje = 'No se cambio el estado...'
		                END
                 END
                GO

            ");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
