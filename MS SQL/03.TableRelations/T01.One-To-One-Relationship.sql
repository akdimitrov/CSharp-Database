CREATE TABLE	Passports
(
	PassportID INT PRIMARY KEY IDENTITY(101,1),
	PassportNumber CHAR(8) UNIQUE NOT NULL
)
GO

CREATE TABLE Persons
(
	PesronID INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30) NOT NULL,
	Salary DECIMAL(9,2) NOT NULL,
	PassportID INT UNIQUE REFERENCES Passports(PassportID) NOT NULL
)
GO

INSERT INTO Passports(PassportNumber) 
	VALUES
	('N34FG21B'),
	('K65LO4R7'),
	('ZE657QP2')
GO

INSERT INTO Persons(FirstName,Salary,PassportID) 
	VALUES
	('Roberto', 43300,102),
	('Tom', 56100,103),
	('Yana', 60200,101)
GO

SELECT *
	FROM Passports

SELECT *
	FROM Persons