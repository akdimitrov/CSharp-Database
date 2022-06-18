CREATE PROC usp_DepositMoney(@AccountId INT, @MoneyAmount DECIMAL(18,4))
AS
BEGIN TRANSACTION
	IF (@MoneyAmount < 0)
		BEGIN
			ROLLBACK;
			THROW 50001, 'Negative amount', 16
		END

	IF ((SELECT COUNT(*) FROM Accounts WHERE @AccountId = AccountHolderId) <= 0)
		BEGIN
			ROLLBACK;
			THROW 50002, 'Invalid account', 16
		END
	
	UPDATE Accounts
		SET Balance += ROUND(@MoneyAmount, 4)
		WHERE Id = @AccountId
	COMMIT
GO
