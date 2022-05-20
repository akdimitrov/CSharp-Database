USE Hotel

UPDATE Payments
SET TaxRate *= 0.97

SELECT TaxRate From Payments

TRUNCATE TABLE Occupancies