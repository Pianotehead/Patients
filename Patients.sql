CREATE DATABASE PatientJournal

CREATE TABLE Patients 
(
	 Id INT IDENTITY PRIMARY KEY,
	 FirstName VARCHAR(50) NOT NULL,
	 LastName VARCHAR(50) NOT NULL,
	 SocialSecurityNumber VARCHAR(50) NOT NULL UNIQUE,
	 PhoneNumber VARCHAR(50),
	 Email VARCHAR(50)
 )
 
 CREATE TABLE Journals (
   Id INT IDENTITY PRIMARY KEY,
   PatientId INT NOT NULL,
   CONSTRAINT FK_Journals_Patients 
    FOREIGN KEY (PatientId)     
    REFERENCES Patients (Id)     
    ON DELETE CASCADE    
)

CREATE TABLE JournalEntries (
   Id INT IDENTITY PRIMARY KEY,
   JournalId INT NOT NULL,
   EntryBy VARCHAR(50) NOT NULL,
   EntryDate DATETIME NOT NULL,
   Entry VARCHAR(50) NOT NULL,
   CONSTRAINT FK_JournalEntries_Journal 
    FOREIGN KEY (JournalId)     
    REFERENCES Journals (Id)     
    ON DELETE CASCADE 
)