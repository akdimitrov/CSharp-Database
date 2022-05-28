SELECT TOP(5)
		c.CountryName, 
		MAX(p.Elevation) AS HighestPeak, 
		MAX(r.[Length]) AS LongestRiver 
	FROM Countries c
	LEFT JOIN MountainsCountries mc ON mc.CountryCode =c.CountryCode
	LEFT JOIN Mountains m ON m.Id = mc.MountainId
	LEFT JOIN Peaks p ON p.MountainId = m.Id
	LEFT JOIN CountriesRivers cr ON cr.CountryCode = c.CountryCode
	LEFT JOIN Rivers r ON r.Id = cr.RiverId
   GROUP BY c.CountryName
   ORDER BY HighestPeak DESC, LongestRiver DESC, c.CountryName