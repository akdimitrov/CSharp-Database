SELECT TOP(10)
	   FirstName,
	   LastName,
	   DepartmentID
	FROM Employees e
	WHERE e.Salary > 
			(SELECT 
			AVG(Salary) 
			FROM Employees emp
			GROUP BY DepartmentID
			HAVING e.DepartmentID = emp.DepartmentID)
	ORDER BY e.DepartmentID