CREATE TABLE Medarbejder
(
ID int NOT NULL PRIMARY KEY,
Fornavn varchar(255),
Efternavn varchar(255),
SalleryID int FOREIGN KEY REFERENCES Sallery(SalleryID),
);CREATE TABLE Sallery(SalleryID int NOT NULL PRIMARY KEY,Navn varchar(255),amount int,)CREATE TABLE Dept(PersonID int NOT NULL PRIMARY KEY,amount int,)INSERT INTO Sallery (SalleryID, Navn, Amount)VALUES (1, 'Slaveløn', 100)INSERT INTO Sallery (SalleryID, Navn, Amount)VALUES (2, 'Elevløn', 150)INSERT INTO Sallery (SalleryID, Navn, Amount)VALUES (1, 'Mesterløn', 300)INSERT INTO Sallery (SalleryID, Navn, Amount)VALUES (1, 'ChefLøn', 1000)