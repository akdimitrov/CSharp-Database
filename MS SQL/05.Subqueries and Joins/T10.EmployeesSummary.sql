SELECT TOP(50)
	   e.EmployeeID, 
	   CONCAT_WS(' ', e.FirstName, e.LastName) AS EmployeeName,
	   CONCAT_WS(' ', m.FirstName, m.LastName) AS ManagerName,
	   d.[Name] AS DepartmentName
	FROM Employees e
	JOIN Employees m ON m.EmployeeID = e.ManagerID
	JOIN Departments d ON d.DepartmentID = e.DepartmentID
	ORDER BY EmployeeID
