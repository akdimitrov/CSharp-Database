SELECT COUNT(*) as [Count]
	FROM Countries c
	LEFT JOIN MountainsCountries mc ON mc.CountryCode = c.CountryCode
	WHERE mc.CountryCode IS NULL