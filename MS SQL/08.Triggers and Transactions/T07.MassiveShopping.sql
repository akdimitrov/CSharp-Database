DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = 'Stamat');
DECLARE @GameId INT = (SELECT Id FROM Games WHERE [Name] = 'Safflower');
DECLARE @UserGameId INT = (SELECT Id FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId);

DECLARE @StamatCash DECIMAL (15,2) = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId);
DECLARE @ItemsPrice DECIMAL (15,2) = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 11 AND 12);
IF (@StamatCash >= @ItemsPrice)
BEGIN
	BEGIN TRANSACTION
		UPDATE UsersGames
		SET Cash -= @ItemsPrice
		WHERE Id = @UserGameId

		INSERT INTO UserGameItems (ItemId, UserGameId)
		SELECT ID, @UserGameId FROM Items WHERE MinLevel  BETWEEN 11 AND 12
	COMMIT
END

SET @StamatCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId);
SET @ItemsPrice = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 19 AND 21);
IF (@StamatCash >= @ItemsPrice)
BEGIN
	BEGIN TRANSACTION
		UPDATE UsersGames
		SET Cash -= @ItemsPrice
		WHERE Id = @UserGameId

		INSERT INTO UserGameItems (ItemId, UserGameId)
		SELECT ID, @UserGameId FROM Items WHERE MinLevel BETWEEN 19 AND 21
	COMMIT
END

SELECT i.[Name] AS [Item Name]
	FROM Users u
	JOIN UsersGames ug ON u.Id = ug.UserId
	JOIN Games g ON g.Id = ug.GameId
	JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
	JOIN Items i ON i.Id = ugi.ItemId
	WHERE u.Username = 'Stamat' AND g.[Name] = 'Safflower'
	ORDER BY i.[Name]