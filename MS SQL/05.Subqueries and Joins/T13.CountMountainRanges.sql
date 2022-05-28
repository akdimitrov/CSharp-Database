SELECT CountryCode, COUNT(*) AS MountainRanges
	FROM MountainsCountries
	WHERE CountryCode IN ('BG', 'RU', 'US')
	GROUP BY CountryCode