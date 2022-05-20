CREATE DATABASE CarRental
USE CarRental

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(30) NOT NULL,
	DailyRate DECIMAL (6,2) NOT NULL,
	WeeklyRate DECIMAL (8,2) NOT NULL,
	MonthlyRate DECIMAL (10,2) NOT NULL,
	WeekendRate DECIMAL (6,2) NOT NULL
)

INSERT INTO Categories VALUES
('Budget', 10, 50, 100, 12.5),
('Regular', 15, 70, 150, 19.5),
('Luxury', 49.99, 300, 999.90, 89.99)

CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber VARCHAR(10) UNIQUE NOT NULL,
	Manufacturer VARCHAR(30) NOT NULL,
	Model VARCHAR(30) NOT NULL,
	CarYear SMALLINT NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors TINYINT NOT NULL,
	Picture VARCHAR(MAX),
	Condition VARCHAR(30) NOT NULL,
	Available BIT NOT NULL
)

INSERT INTO Cars VALUES
('PB6666AC', 'BMW', 'M5', 2015, 3, 5, NULL,'New', 0),
('CA1232BB', 'Trabant', '55', 1980, 1, 3, NULL,'EverGreen', 1),
('PB0321AH', 'VW', 'Passat', 2005, 2, 5, NULL,'Used', 1)


CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(39) NOT NULL,
	Title VARCHAR(30) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Employees VALUES
('Peter', 'Petrov', 'Manager', NULL),
('George', 'KaraPetkov', 'Intern', NULL),
('Ivan', 'Dimitrov', 'TeamLead', NULL)


CREATE TABLE Customers
(
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenceNumber VARCHAR(30) NOT NULL,
	FullName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(60) NOT NULL,
	City VARCHAR(30) NOT NULL,
	ZIPCode VARCHAR(20) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Customers VALUES
(123321123321, 'Anton Nikolaev', 'ul. Dolna Gorna 5', 'Novo Selo', '443344', NULL),
(456677774554, 'Misho Mishov', 'ul. Zahari Stoyanov', 'Varna', '666666', NULL),
(987677781122, 'Zahari Iliev', 'ul. Knyaz Boris', 'Sofia', '120000', NULL)


CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
	CarId INT FOREIGN KEY REFERENCES Cars(Id),
	TankLevel FLOAT(2) NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage AS (KilometrageEnd - KilometrageStart),
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	TotalDays AS DATEDIFF(Day, StartDate, EndDate),
	RateApplied DECIMAL(10,2) NOT NULL,
	TaxRate DECIMAL (5,2) NOT NULL,
	OrderStatus VARCHAR(30) NOT NULL,
	Notes VARCHAR(MAX),
)

INSERT INTO RentalOrders VALUES
(1, 1, 1, 75, 0, 100, '05/14/2022', '05/15/2022', 10, 20.00, 'Paid', NULL),
(2, 2, 2, 55, 202158, 203158, '05/13/2022', '05/16/2022', 70, 20.00,'Paid', NULL),
(3, 3, 3, 13, 79800, 79950, '05/14/2022', '05/15/2022', 49.99, 20.00,'Paid', NULL)

SELECT * FROM RentalOrders