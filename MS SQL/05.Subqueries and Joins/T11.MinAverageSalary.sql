SELECT MIN(a.AverageSalary) as MinAverageSalary
	FROM 
	(
		SELECT e.DepartmentID, AVG(e.Salary) as AverageSalary
	    FROM Employees e
	    GROUP BY e.DepartmentID
	) as a

SELECT TOP(1) AVG(Salary) as MinAverageSalary
	FROM Employees 
	GROUP BY DepartmentID
	ORDER BY MinAverageSalary