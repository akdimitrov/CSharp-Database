SELECT MountainRange, PeakName, Elevation 
	FROM Peaks
	JOIN Mountains ON Mountains.ID = Peaks.MountainID
	WHERE MountainRange = 'Rila'
	ORDER BY Elevation DESC
