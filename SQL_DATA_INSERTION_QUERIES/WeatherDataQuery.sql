-- Step 1: Drop and ReCreate the database

-- drops table if exist
IF OBJECT_ID('dbo.WeatherMeasurements', 'U') IS NOT NULL
    DROP TABLE dbo.WeatherMeasurements;
GO

-- drops table if exist
IF OBJECT_ID('dbo.Locations', 'U') IS NOT NULL
    DROP TABLE dbo.Locations;
GO

-- create table if doe snot exist already
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MauiAppDB')
BEGIN
    CREATE DATABASE MauiAppDB;
END

-- Use the database
USE MauiAppDB;
GO

-- Step 2: Create Locations table with Name column
CREATE TABLE Locations (
    LocationId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Latitude FLOAT NOT NULL,
    Longitude FLOAT NOT NULL,
    Elevation INT NOT NULL,
    UtcOffset INT NOT NULL,
    Timezone NVARCHAR(100) NOT NULL
);
GO

-- Step 3: Create WeatherMeasurements table
CREATE TABLE WeatherMeasurements (
    MeasurementId INT PRIMARY KEY IDENTITY,
    LocationId INT NOT NULL FOREIGN KEY REFERENCES Locations(LocationId) ON DELETE CASCADE,
    Time DATETIME NULL,
    Temperature FLOAT NOT NULL,
    Humidity INT NOT NULL,
    WindSpeed FLOAT NOT NULL,
    WindDirection INT NOT NULL
);
GO

-- Insert sample Scottish locations with names
INSERT INTO Locations (Name, Latitude, Longitude, Elevation, UtcOffset, Timezone)
VALUES 
('Edinburgh', 55.9533, -3.1883, 47, 0, 'GMT'),
('Aberdeen', 57.1497, -2.0943, 20, 0, 'GMT'),
('Stirling', 56.4907, -4.2026, 100, 0, 'GMT'),
('Glasgow', 55.8642, -4.2518, 40, 0, 'GMT'),
('Thurso', 58.9690, -3.3050, 15, 0, 'GMT'),
('Dundee', 56.4620, -2.9707, 80, 0, 'GMT'),
('Inverness', 57.4778, -4.2247, 10, 0, 'GMT'),
('Perth', 56.3969, -3.4375, 40, 0, 'GMT'),
('Ayr', 55.4604, -4.6290, 20, 0, 'GMT'),
('Oban', 56.4101, -5.4723, 10, 0, 'GMT'),
('Fort William', 56.8200, -5.1139, 100, 0, 'GMT'),
('Kirkwall', 58.9790, -2.9590, 15, 0, 'GMT'),
('Isle of Skye', 57.4110, -5.2000, 50, 0, 'GMT'),
('St Andrews', 56.3409, -2.8037, 30, 0, 'GMT'),
('Campbeltown', 55.4260, -5.6020, 10, 0, 'GMT');

-- Edinburgh (LocationId = 1)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(1, '2025-04-24 08:00', 9.1, 82, 5.6, 90),
(1, '2025-04-24 12:00', 11.4, 77, 6.2, 120),
(1, '2025-04-24 16:00', 12.8, 74, 4.9, 130),
(1, '2025-04-24 20:00', 10.2, 85, 3.3, 100),
(1, '2025-04-25 08:00', 8.9, 88, 5.0, 80),
(1, '2025-04-25 12:00', 10.5, 80, 4.5, 90);

-- Aberdeen (LocationId = 2)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(2, '2025-04-24 08:00', 7.5, 92, 6.1, 100),
(2, '2025-04-24 12:00', 9.2, 85, 5.8, 110),
(2, '2025-04-24 16:00', 10.1, 81, 5.2, 115),
(2, '2025-04-24 20:00', 8.7, 89, 4.0, 95),
(2, '2025-04-25 08:00', 6.8, 93, 4.9, 70),
(2, '2025-04-25 12:00', 8.4, 87, 5.1, 85);

-- Stirling (LocationId = 3)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(3, '2025-04-24 08:00', 6.9, 91, 5.7, 140),
(3, '2025-04-24 12:00', 9.3, 84, 6.4, 150),
(3, '2025-04-24 16:00', 10.5, 80, 5.6, 160),
(3, '2025-04-24 20:00', 7.8, 88, 3.9, 145),
(3, '2025-04-25 08:00', 6.3, 95, 4.2, 130),
(3, '2025-04-25 12:00', 8.7, 89, 4.5, 135);

-- Glasgow (LocationId = 4)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(4, '2025-04-24 08:00', 8.2, 86, 5.0, 100),
(4, '2025-04-24 12:00', 10.9, 79, 5.5, 110),
(4, '2025-04-24 16:00', 11.6, 76, 4.3, 125),
(4, '2025-04-24 20:00', 9.1, 84, 3.6, 105),
(4, '2025-04-25 08:00', 7.6, 90, 4.1, 95),
(4, '2025-04-25 12:00', 9.8, 83, 4.8, 100);

-- Thurso (LocationId = 5)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(5, '2025-04-24 08:00', 5.4, 94, 6.3, 80),
(5, '2025-04-24 12:00', 7.1, 89, 6.7, 100),
(5, '2025-04-24 16:00', 8.0, 85, 5.8, 110),
(5, '2025-04-24 20:00', 6.6, 91, 4.7, 95),
(5, '2025-04-25 08:00', 5.1, 96, 5.5, 90),
(5, '2025-04-25 12:00', 6.9, 88, 5.9, 105);

-- Dundee (LocationId = 6)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(6, '2025-04-24 08:00', 8.5, 85, 5.0, 95),
(6, '2025-04-24 12:00', 10.2, 78, 5.6, 100),
(6, '2025-04-24 16:00', 11.0, 74, 6.1, 105),
(6, '2025-04-24 20:00', 9.3, 80, 4.8, 110);

-- Inverness (LocationId = 7)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(7, '2025-04-24 08:00', 6.2, 90, 6.4, 115),
(7, '2025-04-24 12:00', 8.0, 84, 5.2, 120),
(7, '2025-04-24 16:00', 9.3, 79, 5.7, 125),
(7, '2025-04-24 20:00', 7.4, 85, 4.3, 110);

-- Perth (LocationId = 8)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(8, '2025-04-24 08:00', 9.1, 87, 5.1, 105),
(8, '2025-04-24 12:00', 10.4, 81, 5.9, 110),
(8, '2025-04-24 16:00', 11.2, 77, 6.2, 115),
(8, '2025-04-24 20:00', 9.5, 83, 5.0, 120);

--  Ayr (LocationId = 9)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(9, '2025-04-24 08:00', 7.8, 89, 5.5, 95),
(9, '2025-04-24 12:00', 9.0, 81, 6.0, 100),
(9, '2025-04-24 16:00', 10.1, 77, 5.3, 105),
(9, '2025-04-24 20:00', 8.4, 84, 4.7, 110);

--  Oban (LocationId = 10)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(10, '2025-04-24 08:00', 6.5, 92, 5.3, 100),
(10, '2025-04-24 12:00', 8.2, 88, 5.8, 110),
(10, '2025-04-24 16:00', 9.0, 82, 6.1, 120),
(10, '2025-04-24 20:00', 7.3, 85, 5.0, 115);

-- Fort William (LocationId = 11)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(11, '2025-04-24 08:00', 5.6, 94, 5.0, 115),
(11, '2025-04-24 12:00', 7.0, 90, 5.7, 120),
(11, '2025-04-24 16:00', 8.3, 85, 6.2, 125),
(11, '2025-04-24 20:00', 6.5, 89, 4.8, 130);

-- Kirkwall (LocationId = 12)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(12, '2025-04-24 08:00', 7.2, 91, 5.4, 105),
(12, '2025-04-24 12:00', 9.1, 85, 5.9, 110),
(12, '2025-04-24 16:00', 10.4, 80, 6.1, 115),
(12, '2025-04-24 20:00', 8.7, 83, 5.3, 120);

-- Isle of Skye (LocationId = 13)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(13, '2025-04-24 08:00', 6.0, 93, 5.2, 125),
(13, '2025-04-24 12:00', 7.4, 88, 5.8, 130),
(13, '2025-04-24 16:00', 8.2, 82, 6.0, 135),
(13, '2025-04-24 20:00', 7.0, 85, 4.9, 140);

-- St Andrews (LocationId = 14)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(14, '2025-04-24 08:00', 7.5, 90, 5.3, 100),
(14, '2025-04-24 12:00', 9.2, 84, 5.7, 110),
(14, '2025-04-24 16:00', 10.3, 80, 6.1, 120),
(14, '2025-04-24 20:00', 8.1, 86, 4.8, 115);

--  Campbeltown (LocationId = 15)
INSERT INTO WeatherMeasurements (LocationId, Time, Temperature, Humidity, WindSpeed, WindDirection)
VALUES
(15, '2025-04-24 08:00', 8.0, 85, 5.0, 95),
(15, '2025-04-24 12:00', 9.5, 78, 5.5, 100),
(15, '2025-04-24 16:00', 10.7, 74, 6.0, 105),
(15, '2025-04-24 20:00', 8.8, 80, 4.7, 110);
