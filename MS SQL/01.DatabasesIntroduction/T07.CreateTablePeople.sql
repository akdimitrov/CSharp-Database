CREATE TABLE People
(
	Id Int PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX) CHECK (DATALENGTH(Picture) <= 2097152),
	Height FLOAT(2),
	[Weight] FLOAT(2),
	Gender CHAR(1) CHECK (gender = 'f' or gender = 'm') NOT NULL,
	Birthdate DATETIME NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People ([Name], Gender, Birthdate) VALUES
('Ivan', 'm', 28-11-1992),
('Georgre', 'm', 15-10-1991),
('Floo', 'f', 5-7-1990),
('Madelane', 'f', 3-12-1994),
('Christina', 'f', 26-9-1996)

DROP TABLE People