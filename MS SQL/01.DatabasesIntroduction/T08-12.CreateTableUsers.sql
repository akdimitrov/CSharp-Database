CREATE TABLE Users 
(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX) CHECK (DATALENGTH(ProfilePicture) <= 900 * 1024),
	LastLoginTime DATETIME,
	IsDeleted BIT
)

INSERT INTO Users (Username, [Password]) VALUES
('TheGOD', 'AINTnoDEvil'),
('noname', '1234dksm'),
('Lool123', 'wsddd223dd'),
('Ivan', '1122334567789'),
('ironmaden', 'qwerty')

ALTER TABLE Users 
DROP PK__Users__3214EC07A04C2BCC
 
ALTER TABLE Users 
ADD CONSTRAINT PK_IdUsername PRIMARY KEY (Id,Username)

ALTER TABLE Users
ADD CHECK (LEN(Password) >= 5)

ALTER TABLE Users
ADD DEFAULT GETDATE() FOR LastLoginTime 

ALTER TABLE Users 
DROP PK_IdUsername

ALTER TABLE Users 
ADD PRIMARY KEY (Id)

ALTER TABLE Users
ADD CHECK (LEN(Username) >= 3)