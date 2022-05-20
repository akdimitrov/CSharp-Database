--Problem 16.	Create SoftUni Database

CREATE DATABASE SoftUni
USE SoftUni

CREATE TABLE Towns
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY,
	AddressText VARCHAR(200) NOT NULL,
	TownId INT FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	MiddleName VARCHAR(30),
	LastName VARCHAR(30) NOT NULL,
	JobTitle VARCHAR(30) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id),
	HireDate DATE NOT NULL,
	Salary DECIMAL (10,2) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
)

--Problem 17.	Backup Database - DONE
--Problem 18.	Basic Insert

INSERT INTO Towns VALUES ('Sofia')
INSERT INTO Towns VALUES ('Plovdiv')
INSERT INTO Towns VALUES ('Varna')
INSERT INTO Towns VALUES ('Burgas')

INSERT INTO Departments VALUES ('Engineering')
INSERT INTO Departments VALUES ('Sales')
INSERT INTO Departments VALUES ('Marketing')
INSERT INTO Departments VALUES ('Software Development')
INSERT INTO Departments VALUES ('Quality Assurance')

INSERT INTO Employees VALUES
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '02/01/2013', 3500.00, NULL),
('Petar', 'Petrov', 'Petrov', 'Senior Engineer', 1, '03/02/2004', 4000.00, NULL),
('Maria', 'Petrova', 'Ivanova', 'Intern', 5, '08/28/2016', 525.25, NULL),
('Georgi', 'Teziev', 'Ivanov', 'CEO', 2, '12/09/2007', 3000.00, NULL),
('Peter', 'Pan', 'Pan', 'Intern', 3, '08/28/2016', 599.88, NULL)

--Problem 19.	Basic Select All Fields

SELECT * FROM Towns
SELECT * FROM Departments
SELECT * FROM Employees

--Problem 20.	Basic Select All Fields and Order Them

SELECT * 
	FROM Towns 
	ORDER BY [Name] ASC

SELECT * 
	FROM Departments 
	ORDER BY [Name] ASC

SELECT * 
	FROM Employees 
	ORDER BY Salary DESC

--Problem 21.	Basic Select Some Fields

SELECT [Name] 
	FROM Towns 
	ORDER BY [Name] ASC

SELECT [Name] 
	FROM Departments 
	ORDER BY [Name] ASC

SELECT FirstName, LastName, JobTitle, Salary 
	FROM Employees 
	ORDER BY Salary DESC

--Problem 22.	Increase Employees Salary

UPDATE Employees
SET Salary *= 1.1

SELECT Salary FROM Employees