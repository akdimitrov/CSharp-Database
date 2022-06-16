CREATE PROC usp_GetTownsStartingWith(@Substring NVARCHAR(20))
AS
	SELECT [Name]
		FROM Towns
		WHERE LEFT([Name], LEN(@Substring)) = @Substring
GO

EXEC usp_GetTownsStartingWith 'b'