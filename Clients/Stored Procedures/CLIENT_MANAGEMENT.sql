-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.STPR_CLIENTES_PRUEBA_MANTENIMIENTO
	-- Add the parameters for the stored procedure here
	@id integer, @nombre_completo varchar(200), @identificacion numeric, @telefono numeric, @operation varchar(1), @result int OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	set @operation = LOWER(@operation);
	set @result = 0;
	declare @max_decimal decimal(10,0)
	set @max_decimal = 9999999999;

	BEGIN TRY
		if @identificacion <> null AND (@identificacion > @max_decimal OR @identificacion < 0)
			set @result = 2
		ELSE
			if @telefono <> null AND (@telefono > @max_decimal OR @telefono < 0) 
				set @result = 3
			ELSE
				IF @operation = 'i'
				-- Insert statements for procedure here
					INSERT INTO dbo.CLIENTES(NOMBRE_COMPLETO,IDENTIFICACION,TELEFONO) VALUES (@nombre_completo,@identificacion,@telefono);
				ELSE
					IF @operation = 'a'
						UPDATE dbo.CLIENTES SET NOMBRE_COMPLETO = @nombre_completo, IDENTIFICACION = @identificacion, TELEFONO = @telefono where ID = @id;
					ELSE
						IF @operation = 'b'
							DELETE FROM dbo.CLIENTES WHERE ID = @id;
	END TRY
	BEGIN CATCH
		DECLARE @error INT;
		SELECT @error = ERROR_NUMBER();
		IF @error = 2627 OR @error = 2601
			set @result = 1;
	END CATCH	 
END
GO
