CREATE FUNCTION ufn_CashInUsersGames(@GameName NVARCHAR(MAX))
RETURNS TABLE
AS
	RETURN (SELECT SUM(k.Cash) AS TotalCash
		FROM (SELECT Cash,
			ROW_NUMBER() OVER (ORDER BY Cash DESC) AS RowNumber
		FROM Games AS g
		JOIN UsersGames AS ug ON ug.GameId = g.Id
		WHERE g.[Name] = @GameName) AS k
		WHERE k.RowNumber % 2 = 1)
GO

SELECT * FROM ufn_CashInUsersGames('Love in a mist')