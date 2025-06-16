-- Script de ejemplo para inicializar la base de datos
CREATE TABLE Orders (
    Id INT IDENTITY PRIMARY KEY,
    Client NVARCHAR(100) NOT NULL,
    Product NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    Origin_Latitude FLOAT NOT NULL,
    Origin_Longitude FLOAT NOT NULL,
    Destination_Latitude FLOAT NOT NULL,
    Destination_Longitude FLOAT NOT NULL,
    DistanceKm FLOAT NOT NULL,
    EstimatedCost DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
);