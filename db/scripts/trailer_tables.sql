-- Trailer and TrailerAssignment table creation scripts for TrailerManagement module

IF OBJECT_ID('dbo.tbl_Trailer', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.tbl_Trailer;
END;
GO

CREATE TABLE dbo.tbl_Trailer
(
    TrailerId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tbl_Trailer PRIMARY KEY,
    TrailerPlate   NVARCHAR(20) NOT NULL,
    TrailerTypeId  INT NOT NULL,
    OwnerTypeId    INT NOT NULL,
    Brand          NVARCHAR(100) NULL,
    Model          NVARCHAR(100) NULL,
    ModelYear      INT NULL,
    Capacity       NVARCHAR(50) NULL,
    AxleCount      INT NULL,
    ChassisNo      NVARCHAR(100) NULL,
    Description    NVARCHAR(500) NULL,
    IsActive       BIT NOT NULL CONSTRAINT DF_tbl_Trailer_IsActive DEFAULT(1),
    CreatedAt      DATETIME2(7) NOT NULL CONSTRAINT DF_tbl_Trailer_CreatedAt DEFAULT(SYSDATETIME()),
    UpdatedAt      DATETIME2(7) NOT NULL CONSTRAINT DF_tbl_Trailer_UpdatedAt DEFAULT(SYSDATETIME())
);
GO

CREATE UNIQUE INDEX IX_tbl_Trailer_TrailerPlate ON dbo.tbl_Trailer(TrailerPlate);
CREATE INDEX IX_tbl_Trailer_TrailerTypeId ON dbo.tbl_Trailer(TrailerTypeId);
CREATE INDEX IX_tbl_Trailer_OwnerTypeId ON dbo.tbl_Trailer(OwnerTypeId);
CREATE INDEX IX_tbl_Trailer_IsActive ON dbo.tbl_Trailer(IsActive);
GO

IF OBJECT_ID('dbo.tbl_TrailerAssignment', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.tbl_TrailerAssignment;
END;
GO

CREATE TABLE dbo.tbl_TrailerAssignment
(
    AssignmentId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tbl_TrailerAssignment PRIMARY KEY,
    TrailerId    INT NOT NULL,
    VehicleId    INT NOT NULL,
    StartDate    DATE NOT NULL,
    EndDate      DATE NULL,
    Notes        NVARCHAR(500) NULL,
    CreatedAt    DATETIME2(7) NOT NULL CONSTRAINT DF_tbl_TrailerAssignment_CreatedAt DEFAULT(SYSDATETIME()),
    CONSTRAINT FK_tbl_TrailerAssignment_Trailer FOREIGN KEY (TrailerId) REFERENCES dbo.tbl_Trailer(TrailerId),
    CONSTRAINT FK_tbl_TrailerAssignment_Vehicle FOREIGN KEY (VehicleId) REFERENCES dbo.tbl_Vehicle(Vehicle_id),
    CONSTRAINT CK_tbl_TrailerAssignment_EndDate CHECK (EndDate IS NULL OR EndDate >= StartDate)
);
GO

CREATE UNIQUE INDEX UX_tbl_TrailerAssignment_ActiveTrailer
    ON dbo.tbl_TrailerAssignment(TrailerId)
    WHERE EndDate IS NULL;
CREATE UNIQUE INDEX UX_tbl_TrailerAssignment_ActiveVehicle
    ON dbo.tbl_TrailerAssignment(VehicleId)
    WHERE EndDate IS NULL;
GO
