SELECT SUM(G.DepositAmount - H.DepositAmount) AS SumDifference
	FROM WizzardDeposits AS H
	JOIN WizzardDeposits AS G ON H.Id = G.Id + 1