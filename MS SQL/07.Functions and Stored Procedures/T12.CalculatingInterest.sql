CREATE PROC usp_CalculateFutureValueForAccount(@AccountID INT, @InterestRate FLOAT)
AS
	SELECT ah.Id AS [Account Id],
	       ah.FirstName AS [First Name],
		   ah.LastName AS [Last Name],
		   a.Balance AS [Current Balance],
		   dbo.ufn_CalculateFutureValue(a.Balance, @InterestRate, 5)
		FROM Accounts a
		JOIN AccountHolders ah ON a.AccountHolderId = ah.Id
		WHERE a.Id = @AccountID
GO

EXEC usp_CalculateFutureValueForAccount 1, 0.1