CREATE DATABASE Hotel
USE Hotel

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	Title VARCHAR(30) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Employees VALUES
('John', 'Gone', 'Manager', NULL),
('Stephen', 'Johnson', 'Deputee', NULL),
('Michael', 'Owen', 'TeamLead', NULL)

CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	PhoneNumber CHAR(10) NOT NULL,
	EmergencyName VARCHAR(60) NOT NULL,
	EmergencyNumber CHAR(10) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Customers VALUES
(100566, 'Alex', 'Balwdin', 0882139771, 'Helen', 0885522877, NULL),
(112366, 'Gregg', 'Taylor', 0885599321, 'Alen', 0885522342, NULL),
(433266, 'Susan', 'Purtill', 0885599312, 'Sam', 0885543331, NULL)

CREATE TABLE RoomStatus
(
	RoomStatus VARCHAR(20) PRIMARY KEY,
	Notes VARCHAR(MAX),
)

INSERT INTO RoomStatus VALUES
('cleaned', NULL),
('reserved', NULL),
('free', NULL)

CREATE TABLE RoomTypes
(
	RoomType VARCHAR(20) PRIMARY KEY,
	Notes VARCHAR(MAX),
)

INSERT INTO RoomTypes VALUES
('one-bed', NULL),
('two-bed', NULL),
('apartment', NULL)

CREATE TABLE BedTypes
(
	BedType VARCHAR(20) PRIMARY KEY,
	Notes VARCHAR(MAX),
)

INSERT INTO BedTypes VALUES
('single', NULL),
('double', NULL),
('queens', NULL)

CREATE TABLE Rooms
(
	RoomNumber SMALLINT PRIMARY KEY,
	RoomType VARCHAR(20) FOREIGN KEY REFERENCES RoomTypes(RoomType),
	BedType VARCHAR(20) FOREIGN KEY REFERENCES BedTypes(BedType),
	Rate REAL,
	RoomStatus VARCHAR(20) FOREIGN KEY REFERENCES RoomStatus(RoomStatus),
	Notes VARCHAR(MAX),
)

INSERT INTO Rooms VALUES
(1, 'one-bed', 'single', 5.5, 'cleaned', NULL),
(2, 'two-bed', 'double', 8.5, 'reserved', NULL),
(3, 'apartment', 'queens', 10, 'free', NULL)

CREATE TABLE Payments
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	PaymentDate DATETIME NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	FirstDateOccupied DATETIME NOT NULL,
	LastDateOccupied DATETIME NOT NULL,
	TotalDays AS DATEDIFF(DAY, FirstDateOccupied, LastDateOccupied),
	AmountCharged DECIMAL(8,2) NOT NULL,
	TaxRate DECIMAL (5,2) NOT NULL,
	TaxAmount AS AmountCharged * TaxRate,
	PaymentTotal DECIMAL(8,2) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Payments VALUES
(1, GETDATE(), 100566, '05-19-2022', '05-20-2022', 50, 20, 60, NULL),
(2, GETDATE(), 112366, '05-19-2022', '05-20-2022', 60, 20, 72, NULL),
(3, GETDATE(), 433266, '05-19-2022', '05-20-2022', 70, 20, 84, NULL)
SELECT * FROM Payments

CREATE TABLE Occupancies
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	DateOccupied DATETIME NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	RoomNumber SMALLINT FOREIGN KEY REFERENCES Rooms(RoomNumber),
	RateApplied DECIMAL (8,2) NOT NULL,
	PhoneCharge DECIMAL (5,2),
	Notes VARCHAR(MAX)
)

INSERT INTO Occupancies VALUES
(1, '05-19-2022', 100566, 1, 6, NULL, NULL),
(2, '05-19-2022', 112366, 2, 7.9, NULL, NULL),
(3, '05-19-2022', 433266, 3, 10, NULL, NULL)