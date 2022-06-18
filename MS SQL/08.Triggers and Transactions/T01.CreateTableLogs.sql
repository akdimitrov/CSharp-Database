CREATE TABLE Logs
(
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT FOREIGN KEY REFERENCES Accounts(Id),
	OldSum DECIMAL(15,2),
	NewSum DECIMAL(15,2)
)
GO

CREATE TRIGGER tr_InsertAccountInfo
ON Accounts FOR UPDATE
AS
	INSERT INTO Logs(AccountID, OldSum, NewSum)
	SELECT i.Id, d.Balance, i.Balance
	FROM inserted AS i
	JOIN deleted AS d ON i.Id = d.Id
	WHERE i.Balance != d.Balance
GO