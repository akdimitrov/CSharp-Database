SELECT TOP(5)
	   k.CountryName, 
	   ISNULL(k.PeakName, '(no highest peak)') AS [Highest Peak Name],
	   ISNULL(k.Elevation, 0) AS [Highest Peak Elevation],
	   ISNULL(k.MountainRange, '(no mountain)') AS Mountain
  FROM (
	     SELECT CountryName, 
			    PeakName, 
			    Elevation, 
		 	    MountainRange,
		  	    DENSE_RANK() OVER (PARTITION BY CountryName ORDER BY Elevation DESC) AS Ranked
		   FROM Countries c
		   LEFT JOIN MountainsCountries mc ON mc.CountryCode = c.CountryCode
		   LEFT JOIN Mountains m ON m.Id = mc.MountainId
		   LEFT JOIN Peaks p ON p.MountainId = m.Id
	   ) AS k
 WHERE k.Ranked = 1
 ORDER BY CountryName, PeakName