CREATE PROC usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(30))
AS
	SELECT a.[Name],
		CASE
			WHEN a.OwnerId IS NULL THEN 'For adoption'
			ELSE o.[Name]
		END AS OwnersName
		FROM Animals a
		LEFT JOIN Owners o ON o.Id = a.OwnerId
		WHERE a.[Name] = @AnimalName
GO

EXEC usp_AnimalsWithOwnersOrNot 'Hippo'