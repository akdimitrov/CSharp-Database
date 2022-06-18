CREATE TABLE NotificationEmails
(
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT FOREIGN KEY REFERENCES Accounts(Id), 
	[Subject] VARCHAR(50), 
	Body VARCHAR(MAX)
)
GO

CREATE TRIGGER tr_LogEmail ON Logs FOR INSERT
AS
	INSERT INTO NotificationEmails(Recipient,[Subject],Body)
		SELECT AccountId,
			'Balance change for account: ' + CAST(AccountId AS VARCHAR(20)),
			'On ' + CAST(GETDATE() AS VARCHAR(30)) + ' your balance was changed from ' + CAST(OldSum AS VARCHAR(20)) + ' to ' + CAST(NewSum AS VARCHAR(20)) + '.'
		FROM inserted
GO
