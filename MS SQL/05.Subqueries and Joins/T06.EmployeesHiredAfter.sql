SELECT e.FirstName, e.LastName, e.HireDate, d.[Name] AS DeptName
	FROM Employees e
	JOIN Departments d ON d.DepartmentID = e.DepartmentID
	WHERE HireDate > '1999-1-1' AND d.[Name] IN ('Sales', 'Finance')
	ORDER BY e.HireDate