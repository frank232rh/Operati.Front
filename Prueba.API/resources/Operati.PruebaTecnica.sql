USE [PruebaTecnica]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14/7/2023 03:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NULL,
	[Password] [varbinary](max) NULL,
	[Mail] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Password], [Mail]) VALUES (26, N'Francisco Ruiz', 0x020000003FF51B9BAA10393F5F35F7B57CE34621302775674B1B96396A085D0C7317F39CCD4DB4CC13A261F007CF56B99DF9AE7BC1874DF88779AAAFEDD5930849203B6F, N'francisco@pruebas.com')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  StoredProcedure [dbo].[SP_User]    Script Date: 14/7/2023 03:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Francisco Daniel Ruiz Hernández
-- Create date: 06/07/2022
-- Description:	SP for user actions
-- =============================================
CREATE PROCEDURE [dbo].[SP_User]
	@option INT = 0,
	@name VARCHAR(150) = NULL,
	@password VARCHAR(15) = NULL,
	@newPassword VARCHAR(15) = NULL,
	@mail VARCHAR(150) = NULL,
	@id INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --Variables
	DECLARE @Success BIT = 0, @Message VARCHAR(50) = NULL, @Afected INT = 0
	DECLARE @Word VARCHAR(15) = N'operati2023';

	--Obtener listado de usuarios
	IF @option = 1
		BEGIN
			SELECT Id, [Name], Mail FROM Users
			ORDER BY Id ASC
		END

	--Agregar un nuevo usuario
	IF @option = 2
		BEGIN
			BEGIN TRY
				DECLARE @PWD VARBINARY(MAX) = ENCRYPTBYPASSPHRASE(@Word,@password, 1, 'autenticador')
				INSERT INTO Users(Name, Password, Mail) VALUES (@name, @PWD, @mail)
				SET @Afected = @@ROWCOUNT
				IF @Afected > 0
					BEGIN
						SET @Success = 1
						SET @Message = 'The user was successfully registered.'
					END
				ELSE
					BEGIN
						SET @Success = 0
						SET @Message = 'The user could not be registered.'
					END
			END TRY
			BEGIN CATCH
				SET @Success = 0
				SET @Message = ERROR_MESSAGE()
			END CATCH
			SELECT @Success Success, @Message Message
		END

	--Modifica los datos de un usuario
	IF @option = 3
		BEGIN
			BEGIN TRY
				DECLARE @BDPWD VARCHAR(MAX) = (SELECT CAST(DECRYPTBYPASSPHRASE( @Word, u.Password , 1, 'autenticador') AS VARCHAR(50))  FROM Users u WHERE Id = @id)
				IF @BDPWD = @password
					BEGIN
						DECLARE @NEWPWD VARBINARY(MAX) = ENCRYPTBYPASSPHRASE(@Word,@newPassword, 1, 'autenticador')
						UPDATE Users 
						SET Name = @name,
						Password = @NEWPWD,
						Mail = @mail
						WHERE Id = @id

						SET @Afected = @@ROWCOUNT
						IF @Afected > 0
							BEGIN
								SET @Success = 1
								SET @Message = 'The user was successfully modified.'
							END
						ELSE
							BEGIN
								SET @Success = 0
								SET @Message = 'The user could not be registered.'
							END
					END
				ELSE
					BEGIN
						SET @Success = 0
						SET @Message = 'The current password does not match.'
					END
			END TRY
			BEGIN CATCH
				SET @Success = 0
				SET @Message = ERROR_MESSAGE()
			END CATCH
			SELECT @Success Success, @Message Message
		END
END
GO
