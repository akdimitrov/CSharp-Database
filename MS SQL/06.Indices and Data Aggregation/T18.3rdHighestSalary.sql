SELECT DepartmentID, Salary AS ThirdHighestSalary
	FROM (SELECT 
			DepartmentID,
			Salary,
			DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS [Rank]
			FROM Employees
			GROUP BY DepartmentID, Salary
		 ) AS Ranking
	WHERE [Rank] = 3