CREATE TABLE People
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	[Picture] VARBINARY(MAX) CHECK (DATALENGTH([Picture]) <= 2097152),
	[Height] DECIMAL(3,2),
	[Weight] DECIMAL(5,2),
	[Gender] CHAR(1) CHECK ([Gender] = 'f' OR [Gender] = 'm') NOT NULL,
	[Birthdate] DATE NOT NULL,
	[Biography] NVARCHAR(MAX)
)

INSERT INTO People ([Name], [Gender], [Birthdate]) VALUES
('Ivan', 'm', '1992-11-28'),
('Georgre', 'm', '1991-10-15'),
('Floo', 'f', '1992-07-09'),
('Madelane', 'f', '1994-12-03'),
('Christina', 'f', '1996-09-26')

DROP TABLE People