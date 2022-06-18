USE SoftUni
GO

CREATE PROC usp_AssignProject(@employeeId INT, @projectId INT)
AS
BEGIN TRANSACTION
	INSERT INTO EmployeesProjects (EmployeeID, ProjectID) VALUES (@employeeId, @projectId)

	IF ((SELECT COUNT(*) FROM EmployeesProjects WHERE EmployeeID = @employeeId) > 3)
	BEGIN
		ROLLBACK
		RAISERROR('The employee has too many projects!', 16,1)
		RETURN
	END
COMMIT

SELECT * FROM EmployeesProjects