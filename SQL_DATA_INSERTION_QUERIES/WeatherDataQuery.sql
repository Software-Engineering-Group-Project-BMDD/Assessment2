-- Step 1: Create the database
CREATE DATABASE MauiAppDB;
GO

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
('Thurso', 58.9690, -3.3050, 15, 0, 'GMT');

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