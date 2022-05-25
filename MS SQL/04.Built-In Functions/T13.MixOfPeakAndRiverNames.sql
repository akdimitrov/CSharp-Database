SELECT PeakName, 
	   RiverName,
	   LOWER(STUFF(RiverName,1 , 1, PeakName)) AS Mix
	FROM Peaks, Rivers
	WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
	ORDER BY Mix