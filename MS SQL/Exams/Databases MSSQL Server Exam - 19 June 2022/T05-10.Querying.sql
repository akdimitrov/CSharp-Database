-- T05.Volunteers
SELECT [Name], PhoneNumber, [Address], AnimalId, DepartmentId 
	FROM Volunteers
	ORDER BY [Name], AnimalId, DepartmentId

-- T06.Animals data
SELECT a.[Name], t.AnimalType, FORMAT(a.BirthDate, 'dd.MM.yyyy') AS BirthDate
	FROM Animals a
	JOIN AnimalTypes t ON t.Id = a.AnimalTypeId
	ORDER BY a.[Name]

-- T07. Owners and Their Animals
SELECT TOP(5) o.[Name] AS [Owner], COUNT(*) AS CountOfAnimals
	FROM Animals a
	JOIN Owners o ON o.Id = a.OwnerId
	GROUP BY a.OwnerId, o.[Name]
	ORDER BY COUNT(*) DESC, o.[Name]

-- T08. Owners, Animals and Cages
SELECT CONCAT(o.[Name],'-',a.[Name]) AS OwnersAnimals,
		o.PhoneNumber,
		ac.CageId
	FROM Owners o
	JOIN Animals a ON a.OwnerId = o.Id
	JOIN AnimalTypes t ON t.Id = a.AnimalTypeId
	JOIN AnimalsCages ac ON ac.AnimalId = a.Id 
	WHERE t.AnimalType = 'mammals'
	ORDER BY o.[Name], a.[Name] DESC

-- T09. Volunteers in Sofia
SELECT v.[Name], 
		v.PhoneNumber,
		SUBSTRING(v.[Address], (CHARINDEX(',', v.[Address]) + 2), LEN(v.[Address])) AS [Address]
	FROM Volunteers v
	JOIN VolunteersDepartments vd ON vd.Id = v.DepartmentId
	WHERE vd.DepartmentName = 'Education program assistant' AND [Address] LIKE '%Sofia%'
	ORDER BY v.[Name]

-- T10. Animals for Adoption
SELECT [Name],
		DATEPART(YEAR,BirthDate) AS BirthYear,
		t.AnimalType
	FROM Animals a
	JOIN AnimalTypes t ON t.Id = a.AnimalTypeId
	WHERE OwnerId IS NULL 
		AND DATEDIFF(YEAR, BirthDate, '01/01/2022') < 5 
		AND t.AnimalType != 'Birds'
	ORDER BY a.[Name]
	