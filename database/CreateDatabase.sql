USE master;
GO

CREATE DATABASE MiniDocumentNotifierDb;
GO

USE MiniDocumentNotifierDb;
GO

CREATE TABLE Institutions (
	Id INT IDENTITY(1,1) NOT NULL,
	Code VARCHAR(5) NOT NULL,
	Name NVARCHAR(100) NOT NULL,

	CONSTRAINT PK_Institutions PRIMARY KEY (Id),
	CONSTRAINT UQ_Institutions_Code UNIQUE (Code)
);
GO

CREATE TABLE Users (
	Id INT IDENTITY(1,1) NOT NULL,
	Username VARCHAR(32) NOT NULL,
	PasswordHash VARCHAR(256) NOT NULL,
	InstitutionId INT NOT NULL,
	IsEnabled BIT NOT NULL CONSTRAINT DF_Users_IsEnabled DEFAULT(1),

	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT UQ_Users_Username UNIQUE (Username),
	CONSTRAINT FK_Users_Institutions FOREIGN KEY (InstitutionId) REFERENCES Institutions(Id)
);
GO

CREATE TABLE Documents (
	Id INT IDENTITY(1,1) NOT NULL,
	InstitutionId INT NOT NULL,
	Name VARCHAR(260) NOT NULL,
	Type INT NOT NULL,
	UploadDate DATETIME2 NOT NULL,
	Status INT NOT NULL,

	CONSTRAINT PK_Documents PRIMARY KEY (Id),
	CONSTRAINT FK_Documents_Institutions FOREIGN KEY (InstitutionId) REFERENCES Institutions(Id),
);
GO

CREATE TABLE ViewConfigurations (
	Id INT IDENTITY(1,1) NOT NULL,
	InstitutionId INT NOT NULL,
	VisibleColumns NVARCHAR(MAX) NOT NULL,
	ActiveCategories NVARCHAR(MAX) NOT NULL,
	LastUpdatedDate DATETIME2 NOT NULL,

	CONSTRAINT PK_ViewConfigurations PRIMARY KEY (Id),
	CONSTRAINT FK_ViewConfigurations_Institutions FOREIGN KEY (InstitutionId) REFERENCES Institutions(Id),
	CONSTRAINT UQ_ViewConfigurations_Institution UNIQUE (InstitutionId),
	CONSTRAINT CK_ViewConfigurations_VisibleColumns_JSON CHECK (ISJSON(VisibleColumns) = 1),
	CONSTRAINT CK_ViewConfigurations_ActiveCategories_JSON CHECK (ISJSON(ActiveCategories) = 1)
);
GO

CREATE INDEX IX_Users_InstitutionId ON Users(InstitutionId);
CREATE INDEX IX_Users_Login ON Users(Username, InstitutionId);
CREATE INDEX IX_Documents_InstitutionId ON Documents(InstitutionId);
CREATE INDEX IX_Documents_Institution_Status ON Documents(InstitutionId, Status);
CREATE INDEX IX_Documents_Institution_Type ON Documents(InstitutionId, Type);
CREATE INDEX IX_ViewConfigurations_InstitutionId ON ViewConfigurations(InstitutionId);
GO