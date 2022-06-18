USE Diablo

CREATE TRIGGER tr_RestrictItems ON UserGameItems INSTEAD OF INSERT
AS
	DECLARE @ItemId INT = (SELECT ItemId FROM inserted);
	DECLARE @UserGameId INT = (SELECT UserGameId FROM inserted);
	DECLARE @ItemMinLevel INT = (SELECT Items.MinLevel FROM Items WHERE Id = @ItemId);
	DECLARE @UserLevel INT = (SELECT [Level] FROM UsersGames WHERE Id = @UserGameId);

	IF (@UserLevel >= @ItemMinLevel)
	BEGIN
		INSERT INTO UserGameItems (ItemId, UserGameId) VALUES (@ItemId, @UserGameId)
	END
GO


UPDATE UsersGames
	SET Cash += 50000
	WHERE GameId = (SELECT Id FROM Games WHERE [Name] = 'Bali') AND
		UserId IN (SELECT Id FROM USERS WHERE Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos'))
GO


CREATE PROC usp_BuyItem (@UserId INT, @ItemId INT, @GameId INT)
AS
BEGIN TRANSACTION
	DECLARE @User INT = (SELECT Id FROM Users WHERE Id = @UserId);
	DECLARE @Item INT = (SELECT Id FROM Items WHERE Id = @ItemId);

	IF (@User IS NULL OR @Item IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Invalid user or item id!', 16,1)
		RETURN
	END

	DECLARE @UserCash DECIMAL(15,2) = (SELECT Cash FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId);
	DECLARE @ItemPrice DECIMAL(15,2) = (SELECT Price FROM Items WHERE Id = @ItemId);

	IF (@UserCash - @ItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Insufficient funds!', 16,2)
		RETURN
	END

	UPDATE UsersGames
	SET Cash -= @ItemPrice
	WHERE UserId = @UserId AND GameId = @GameId

	DECLARE @UserGameId INT = (SELECT Id FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId);
	INSERT INTO UserGameItems (ItemId, UserGameId) VALUES (@ItemId, @UserGameId)

COMMIT
GO


DECLARE @ItemId INT = 251;
WHILE (@ItemId BETWEEN 251 AND 299)
BEGIN

	EXEC usp_BuyItem 12, @ItemId, 212
	EXEC usp_BuyItem 22, @ItemId, 212
	EXEC usp_BuyItem 37, @ItemId, 212
	EXEC usp_BuyItem 52, @ItemId, 212
	EXEC usp_BuyItem 61, @ItemId, 212

	SET @ItemId += 1;
END

SET @ItemId = 501;
WHILE (@ItemId BETWEEN 501 AND 539)
BEGIN

	EXEC usp_BuyItem 12, @ItemId, 212
	EXEC usp_BuyItem 22, @ItemId, 212
	EXEC usp_BuyItem 37, @ItemId, 212
	EXEC usp_BuyItem 52, @ItemId, 212
	EXEC usp_BuyItem 61, @ItemId, 212

	SET @ItemId += 1;
END

SELECT u.Username, g.[Name], ug.Cash, i.[Name]
	FROM Users u
	JOIN UsersGames ug ON ug.UserId = u.Id
	JOIN Games g ON g.Id = ug.GameId
	JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
	JOIN Items i ON i.Id = ugi.ItemId
	WHERE g.[Name] = 'Bali'
	ORDER BY u.Username, i.[Name]