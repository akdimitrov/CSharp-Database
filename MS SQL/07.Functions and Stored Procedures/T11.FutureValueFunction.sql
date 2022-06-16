CREATE FUNCTION ufn_CalculateFutureValue(@Sum DECIMAL(18,4), @AnnualInterestRate FLOAT, @Years INT)
RETURNS DECIMAL (18,4)
AS
BEGIN
	SET @Sum = @Sum * POWER((1 + @AnnualInterestRate), @Years);
	RETURN @Sum; 
END
GO

SELECT dbo.ufn_CalculateFutureValue (1000, 0.1, 5)