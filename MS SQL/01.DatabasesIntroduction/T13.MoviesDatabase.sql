CREATE DATABASE Movies
USE Movies

CREATE TABLE Directors
(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName VARCHAR(50) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Directors (DirectorName) VALUES
('David Cronenberg'),
('Sidney Lumet'),
('Woody Allen'),
('Kathryn Bigelow'),
('Tim Burton')

CREATE TABLE Genres
(
	Id INT PRIMARY KEY IDENTITY,
	GenreName VARCHAR(50) NOT NULL,
	Notes VARCHAR(MAX),
)

INSERT INTO Genres (GenreName) VALUES
('Action'),
('Comedy'),
('Drama'),
('Fantasy'),
('Horror')

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(50) NOT NULL,
	Notes VARCHAR(MAX),
)

INSERT INTO Categories (CategoryName) VALUES
('Feature films'),
('Short films'),
('Animated films'),
('Historical films'),
('Silent films')

CREATE TABLE Movies
(
	Id INT PRIMARY KEY IDENTITY,
	Title VARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear SMALLINT NOT NULL,
	[Length] INT NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating INT,
	Notes VARCHAR(MAX),
)

INSERT INTO Movies (Title, DirectorId, CopyrightYear, [Length], GenreId, CategoryId, Rating) VALUES
('Saw 4', 1, 2000, 120, 1, 1, 8),
('American Pie', 3, 1995, 125, 2, 5, 10),
('Fast and Furious', 2, 2010, 69, 5, 4, 9),
('Alfie', 5, 2001, 73, 3, 3, 6),
('Speed', 4, 2020, 100, 4, 2, 3)

SELECT * FROM Movies
