SELECT c.CountryCode, m.MountainRange, p.PeakName, p.Elevation 
	FROM Peaks p
	JOIN Mountains m ON m.Id = p.MountainId
	JOIN MountainsCountries c ON c.MountainId = m.Id
	WHERE c.CountryCode = 'BG' and p.Elevation > 2835
	ORDER BY p.Elevation DESC