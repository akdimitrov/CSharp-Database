CREATE OR ALTER PROC usp_GetHoldersWithBalanceHigherThan(@Number DECIMAL(18,2))
AS
	SELECT ah.FirstName AS [First Name], ah.LastName AS [Last Name]
		FROM AccountHolders ah
		JOIN Accounts a ON a.AccountHolderId = ah.Id
		GROUP BY ah.FirstName, ah.LastName
		HAVING SUM(a.Balance) > @Number
		ORDER BY ah.FirstName, ah.LastName
GO

EXEC usp_GetHoldersWithBalanceHigherThan 10000
