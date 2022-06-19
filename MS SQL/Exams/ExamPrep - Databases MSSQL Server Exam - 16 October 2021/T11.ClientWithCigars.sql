CREATE FUNCTION udf_ClientWithCigars(@name NVARCHAR(30))
RETURNS INT
AS
BEGIN
	DECLARE @count INT = (SELECT COUNT(*) 
		FROM ClientsCigars
		WHERE ClientId = (SELECT Id FROM Clients WHERE FirstName = @name))
	RETURN @count
END

SELECT dbo.udf_ClientWithCigars('Betty')