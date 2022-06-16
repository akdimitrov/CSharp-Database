CREATE PROC usp_EmployeesBySalaryLevel(@Level VARCHAR(7))
AS
	SELECT FirstName, LastName
		FROM Employees
		WHERE dbo.ufn_GetSalaryLevel(Salary) = @Level
GO

EXEC usp_EmployeesBySalaryLevel 'High'