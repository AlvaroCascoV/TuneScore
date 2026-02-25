
USE TuneScoreDB;
GO

IF OBJECT_ID('Ratings', 'U') IS NOT NULL DROP TABLE Ratings;
IF OBJECT_ID('Songs', 'U') IS NOT NULL DROP TABLE Songs;
IF OBJECT_ID('Albums', 'U') IS NOT NULL DROP TABLE Albums;
IF OBJECT_ID('Artists', 'U') IS NOT NULL DROP TABLE Artists;
IF OBJECT_ID('Genres', 'U') IS NOT NULL DROP TABLE Genres;
IF OBJECT_ID('UserSalts', 'U') IS NOT NULL DROP TABLE UserSalts;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;

-- =========================
-- USERS
-- =========================
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PasswordPlain NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'User',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE UserSalts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    PasswordHash VARBINARY(256) NOT NULL,
    Salt VARBINARY(256) NOT NULL,

    CONSTRAINT FK_UserSalts_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE
);
-- =========================
-- GENRES
-- =========================
CREATE TABLE Genres (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

-- =========================
-- ARTISTS
-- =========================
CREATE TABLE Artists (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    ImageName NVARCHAR(300) NULL
);

-- =========================
-- ALBUMS
-- =========================
CREATE TABLE Albums (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    ReleaseYear INT NOT NULL,
    ArtistId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    ImageName NVARCHAR(300) NULL,
    CONSTRAINT FK_Albums_Artists
        FOREIGN KEY (ArtistId) REFERENCES Artists(Id)
        ON DELETE CASCADE
);

-- =========================
-- SONGS
-- =========================
CREATE TABLE Songs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    DurationSeconds INT NULL,
    AlbumId INT NOT NULL,
    GenreId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Songs_Albums
        FOREIGN KEY (AlbumId) REFERENCES Albums(Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Songs_Genres
        FOREIGN KEY (GenreId) REFERENCES Genres(Id)
);

-- =========================
-- RATINGS
-- =========================
CREATE TABLE Ratings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    SongId INT NOT NULL,
    Score INT NOT NULL CHECK (Score BETWEEN 1 AND 10),
    Comment NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Ratings_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_Ratings_Songs
        FOREIGN KEY (SongId) REFERENCES Songs(Id)
        ON DELETE CASCADE,

    CONSTRAINT UQ_User_Song UNIQUE (UserId, SongId)
);

-- =========================
-- INDEXES (Performance)
-- =========================
CREATE INDEX IX_Albums_ArtistId ON Albums(ArtistId);
CREATE INDEX IX_Songs_AlbumId ON Songs(AlbumId);
CREATE INDEX IX_Songs_GenreId ON Songs(GenreId);
CREATE INDEX IX_Ratings_SongId ON Ratings(SongId);
CREATE INDEX IX_Ratings_UserId ON Ratings(UserId);



USE TuneScoreDB;
GO

-- =========================
-- CLEAN DATABASE
-- =========================

DELETE FROM Ratings;
DELETE FROM Songs;
DELETE FROM Albums;
DELETE FROM Artists;
DELETE FROM Genres;
DELETE FROM Users;
DELETE FROM UserSalts;

DBCC CHECKIDENT ('Users', RESEED, 1);
DBCC CHECKIDENT ('Genres', RESEED, 1);
DBCC CHECKIDENT ('Artists', RESEED, 1);
DBCC CHECKIDENT ('Albums', RESEED, 1);
DBCC CHECKIDENT ('Songs', RESEED, 1);
DBCC CHECKIDENT ('Ratings', RESEED, 1);
DBCC CHECKIDENT ('UserSalts', RESEED, 1);

-- =========================
-- USERS
-- =========================
INSERT INTO Users (Username, Email, PasswordPlain, Role)
VALUES
(N'admin', N'admin@tunescore.com',N'admin123', N'Admin'),
(N'alex', N'alex@email.com', N'alex123', N'User'),
(N'maria', N'maria@email.com', N'maria123', N'User'),
(N'david', N'david@email.com', N'david123', N'User'),
(N'lucia', N'lucia@email.com', N'lucia123', N'User');

-- SALTS (uno por usuario)
INSERT INTO UserSalts (UserId, PasswordHash, Salt)
SELECT 
    Id,
    CONVERT(VARBINARY(256), 'FakeHash_' + Username),
    CONVERT(VARBINARY(256), 'FakeSalt_' + Username)
FROM Users;
-- =========================
-- GENRES
-- =========================
INSERT INTO Genres (Name) VALUES
(N'Rock'),
(N'Pop'),
(N'Hip Hop'),
(N'Electronic'),
(N'Indie');

-- =========================
-- ARTISTS
-- =========================
INSERT INTO Artists (Name, ImageName) VALUES
(N'Neon Skies', N'NeonSkies.jpg'),
(N'Crystal Waves', N'CrystalWaves.jpg'),
(N'Urban Pulse', N'UrbanPulse.jpg'),
(N'Silver Echo', N'SilverEcho.jpg'),
(N'Midnight Drive', N'MidnightDrive.jpg');
-- =========================
-- ALBUMS
INSERT INTO Albums (Title, ReleaseYear, ArtistId, ImageName) 
SELECT N'City Lights', 2021, Id, N'CityLights.jpg'
FROM Artists WHERE Name = N'Neon Skies';

INSERT INTO Albums (Title, ReleaseYear, ArtistId, ImageName) 
SELECT N'Electric Dreams', 2022, Id, N'ElectricDreams.jpg'
FROM Artists WHERE Name = N'Neon Skies';

INSERT INTO Albums (Title, ReleaseYear, ArtistId, ImageName) 
SELECT N'Ocean Signals', 2020, Id, N'OceanSignals.jpg'
FROM Artists WHERE Name = N'Crystal Waves';

INSERT INTO Albums (Title, ReleaseYear, ArtistId, ImageName) 
SELECT N'Night Frequency', 2023, Id, N'NightFrequency.jpg'
FROM Artists WHERE Name = N'Urban Pulse';

INSERT INTO Albums (Title, ReleaseYear, ArtistId, ImageName) 
SELECT N'Parallel Lines', 2021, Id, N'ParallelLines.jpg'
FROM Artists WHERE Name = N'Silver Echo';

INSERT INTO Albums (Title, ReleaseYear, ArtistId, ImageName) 
SELECT N'Endless Roads', 2022, Id, N'EndlessRoads.jpg'
FROM Artists WHERE Name = N'Midnight Drive';

-- =========================
-- SONGS (20)
-- =========================
-- Neon Skies - City Lights
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Neon Horizon', 210, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'City Lights' AND g.Name = N'Rock';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Glass Streets', 185, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'City Lights' AND g.Name = N'Indie';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'After Midnight', 240, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Electric Dreams' AND g.Name = N'Electronic';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Starlight Run', 200, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Electric Dreams' AND g.Name = N'Rock';

-- Crystal Waves - Ocean Signals
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Deep Blue', 230, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Ocean Signals' AND g.Name = N'Electronic';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Tidal Memory', 195, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Ocean Signals' AND g.Name = N'Indie';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Waveform', 205, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Ocean Signals' AND g.Name = N'Electronic';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Sea of Lights', 215, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Ocean Signals' AND g.Name = N'Pop';

-- Urban Pulse - Night Frequency
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Concrete Rhythm', 180, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Night Frequency' AND g.Name = N'Hip Hop';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'City Pulse', 175, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Night Frequency' AND g.Name = N'Hip Hop';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Night Shift', 220, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Night Frequency' AND g.Name = N'Rock';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Last Train Home', 210, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Night Frequency' AND g.Name = N'Pop';

-- Silver Echo - Parallel Lines
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Echo Chamber', 200, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Parallel Lines' AND g.Name = N'Indie';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Fading Signals', 190, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Parallel Lines' AND g.Name = N'Electronic';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Silver Lines', 225, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Parallel Lines' AND g.Name = N'Rock';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Quiet Static', 210, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Parallel Lines' AND g.Name = N'Indie';

-- Midnight Drive - Endless Roads
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Open Highway', 235, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Endless Roads' AND g.Name = N'Rock';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Sunset Motel', 205, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Endless Roads' AND g.Name = N'Pop';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Long Distance', 215, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Endless Roads' AND g.Name = N'Indie';
INSERT INTO Songs (Title, DurationSeconds, AlbumId, GenreId)
SELECT N'Final Exit', 195, a.Id, g.Id
FROM Albums a, Genres g
WHERE a.Title = N'Endless Roads' AND g.Name = N'Rock';

-- =========================
-- RATINGS (5 users x 20 canciones)
-- =========================

DELETE FROM Ratings;
DBCC CHECKIDENT ('Ratings', RESEED, 1);

INSERT INTO Ratings (UserId, SongId, Score, Comment)
SELECT 
    u.Id AS UserId,
    s.Id AS SongId,
    (ABS(CHECKSUM(NEWID())) % 10 + 1) AS Score,  -- genera 1-10 aleatorio
    N'Great song!' AS Comment
FROM Users u
CROSS JOIN Songs s
WHERE u.Role = 'User';


-- =========================
-- SELECTS
-- =========================

SELECT * from Albums
SELECT * from Artists
SELECT * from Genres
SELECT * from Ratings
SELECT * from Songs
SELECT * from Users
SELECT * from UserSalts