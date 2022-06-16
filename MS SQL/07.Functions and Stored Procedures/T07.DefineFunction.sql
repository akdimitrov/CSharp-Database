CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(MAX), @word NVARCHAR(MAX))
RETURNS BIT
AS
BEGIN
	DECLARE @I INT = 1;
	WHILE @I <= LEN(@word)
		BEGIN
			DECLARE @Symbol NCHAR(1) = SUBSTRING(@word, @I, 1);
			IF CHARINDEX(@Symbol, @setOfLetters) = 0
				RETURN 0;
			SET @I += 1;
		END
	RETURN 1;
END
GO

DECLARE @SetOfLetters NVARCHAR(MAX) = 'oistmiahf';
DECLARE @Word NVARCHAR(MAX) = 'Sofia';
SELECT @SetOfLetters, @Word, dbo.ufn_IsWordComprised(@SetOfLetters, @Word) AS Resutlt