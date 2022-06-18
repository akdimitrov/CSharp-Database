CREATE TABLE Deleted_Employees
(
	EmployeeId INT PRIMARY KEY,
	FirstName NVARCHAR(30),
	LastName NVARCHAR(30),
	MiddleName NVARCHAR(30),
	JobTitle NVARCHAR(30),
	DepartmentId INT,
	Salary DECIMAL (15,2)
)
GO

CREATE TRIGGER tr_DeletedEmployees ON Employees FOR DELETE
AS
	INSERT INTO Deleted_Employees (FirstName, LastName, MiddleName, JobTitle, DepartmentId, Salary)
	SELECT FirstName, LastName, MiddleName, JobTitle, DepartmentID, Salary FROM deleted
GO