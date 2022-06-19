CREATE PROC usp_SearchByTaste(@taste VARCHAR(20))
AS
	SELECT c.CigarName,
			'$' + CAST(c.PriceForSingleCigar AS VARCHAR) AS Price,
			t.TasteType,
			b.BrandName,
			CAST(s.[Length] AS VARCHAR) + ' cm' AS CigarLength,
			CAST(s.RingRange AS VARCHAR) + ' cm' AS CigarRingRange
		FROM Cigars c
		JOIN Tastes t ON t.Id = c.TastId
		JOIN Sizes s ON s.Id = c.SizeId
		JOIN Brands b ON b.Id = c.BrandId
		WHERE t.TasteType = @taste
		ORDER BY s.[Length], s.RingRange DESC
GO

EXEC usp_SearchByTaste 'Woody'