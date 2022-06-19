-- T05. Cigars by Price
SELECT CigarName, PriceForSingleCigar, ImageURL 
	FROM Cigars
	ORDER BY PriceForSingleCigar, CigarName DESC

-- T06. Cigars by Taste
SELECT c.Id, c.CigarName, c.PriceForSingleCigar, t.TasteType, t.TasteStrength
	FROM Cigars c
	JOIN Tastes t ON t.Id = c.TastId
	WHERE t.TasteType IN ('Earthy', 'Woody')
	ORDER BY PriceForSingleCigar DESC

-- T07. Clients without Cigars
SELECT Id, FirstName + ' ' + LastName AS ClientName, Email
	FROM Clients
	WHERE Id NOT IN (SELECT ClientId FROM ClientsCigars)
	ORDER BY ClientName

-- T08. First 5 Cigars
SELECT TOP(5) c.CigarName, c.PriceForSingleCigar, c.ImageURL
	FROM Cigars c
	JOIN Sizes s ON s.Id = c.SizeId
	WHERE s.[Length] >= 12 AND (c.CigarName LIKE '%ci%' OR
		(c.PriceForSingleCigar > 50 AND s.RingRange > 2.55))
	ORDER BY c.CigarName, c.PriceForSingleCigar DESC

-- T09. Clients with ZIP Codes
SELECT cl.FirstName + ' ' + cl.LastName AS FullName, 
		a.Country, 
		a.ZIP,
		'$' + CAST(MAX(c.PriceForSingleCigar) AS VARCHAR) AS CigarPrice
	FROM Clients cl
	JOIN Addresses a ON a.Id = cl.AddressId
	JOIN ClientsCigars cc ON cc.ClientId = cl.Id
	JOIN Cigars c ON c.Id = cc.CigarId
	WHERE ISNUMERIC (a.ZIP) = 1
	GROUP BY cl.FirstName, cl.LastName, a.Country, a.ZIP
	ORDER BY FullName

-- T10. Cigars by Size
SELECT cl.LastName, CEILING(AVG(s.Length)) AS CigarLength, CEILING(AVG(s.RingRange)) AS CiagrRingRange
	FROM Clients cl
	JOIN ClientsCigars cc ON cc.ClientId = cl.Id
	JOIN Cigars c ON c.Id = cc.CigarId
	JOIN Sizes s ON s.Id = c.SizeId
	GROUP BY cl.LastName
	ORDER BY CigarLength DESC
