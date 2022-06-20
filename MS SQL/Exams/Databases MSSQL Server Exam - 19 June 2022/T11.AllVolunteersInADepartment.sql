CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(30))
RETURNS INT
AS
BEGIN
	DECLARE @Count INT =
		(SELECT COUNT(*) 
		FROM Volunteers 
		WHERE DepartmentId = 
			(SELECT Id 
			FROM VolunteersDepartments 
			WHERE DepartmentName = @VolunteersDepartment))
	RETURN @Count
END

SELECT dbo.udf_GetVolunteersCountFromADepartment('Education program assistant')