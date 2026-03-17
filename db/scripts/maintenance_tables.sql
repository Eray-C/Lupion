-- Planned Maintenance module DDL and stored procedures

IF OBJECT_ID('dbo.VehicleType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VehicleType
    (
        VehicleTypeId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VehicleType PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Code NVARCHAR(50) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_VehicleType_IsDeleted DEFAULT(0),
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_VehicleType_CreatedAt DEFAULT(SYSDATETIME()),
        UpdatedAt DATETIME2(7) NULL
    );
END;
GO

IF COL_LENGTH('dbo.Vehicles', 'MaintenanceVehicleTypeId') IS NULL
BEGIN
    ALTER TABLE dbo.Vehicles ADD MaintenanceVehicleTypeId INT NULL;
    ALTER TABLE dbo.Vehicles ADD CONSTRAINT FK_Vehicles_MaintenanceVehicleType FOREIGN KEY (MaintenanceVehicleTypeId) REFERENCES dbo.VehicleType(VehicleTypeId);
END;
GO

IF OBJECT_ID('dbo.Component', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Component
    (
        ComponentId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Component PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Code NVARCHAR(50) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Component_IsDeleted DEFAULT(0),
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_Component_CreatedAt DEFAULT(SYSDATETIME())
    );
END;
GO

IF OBJECT_ID('dbo.VehicleComponent', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VehicleComponent
    (
        VehicleComponentId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VehicleComponent PRIMARY KEY,
        VehicleId INT NOT NULL,
        ComponentId INT NOT NULL,
        ParentVehicleComponentId INT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_VehicleComponent_IsDeleted DEFAULT(0),
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_VehicleComponent_CreatedAt DEFAULT(SYSDATETIME()),
        CONSTRAINT FK_VehicleComponent_Vehicle FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles(Id),
        CONSTRAINT FK_VehicleComponent_Component FOREIGN KEY (ComponentId) REFERENCES dbo.Component(ComponentId),
        CONSTRAINT FK_VehicleComponent_Parent FOREIGN KEY (ParentVehicleComponentId) REFERENCES dbo.VehicleComponent(VehicleComponentId)
    );

    CREATE INDEX IX_VehicleComponent_VehicleId ON dbo.VehicleComponent(VehicleId);
END;
GO

IF OBJECT_ID('dbo.MaintenanceJob', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MaintenanceJob
    (
        JobId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MaintenanceJob PRIMARY KEY,
        VehicleTypeId INT NULL,
        VehicleId INT NULL,
        VehicleComponentId INT NULL,
        JobName NVARCHAR(300) NOT NULL,
        PeriodUnit CHAR(1) NOT NULL,
        PeriodAmount INT NOT NULL,
        InitialDueDate DATE NULL,
        LastDoneDate DATE NULL,
        NextDueDate DATE NULL,
        Responsible NVARCHAR(200) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_MaintenanceJob_IsActive DEFAULT(1),
        IsDeleted BIT NOT NULL CONSTRAINT DF_MaintenanceJob_IsDeleted DEFAULT(0),
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_MaintenanceJob_CreatedAt DEFAULT(SYSDATETIME()),
        UpdatedAt DATETIME2(7) NULL,
        CONSTRAINT FK_MaintenanceJob_VehicleType FOREIGN KEY (VehicleTypeId) REFERENCES dbo.VehicleType(VehicleTypeId),
        CONSTRAINT FK_MaintenanceJob_Vehicle FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles(Id),
        CONSTRAINT FK_MaintenanceJob_VehicleComponent FOREIGN KEY (VehicleComponentId) REFERENCES dbo.VehicleComponent(VehicleComponentId),
        CONSTRAINT CK_MaintenanceJob_Target CHECK ((CASE WHEN VehicleTypeId IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN VehicleId IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN VehicleComponentId IS NOT NULL THEN 1 ELSE 0 END) = 1)
    );

    CREATE INDEX IX_MaintenanceJob_NextDueDate ON dbo.MaintenanceJob(NextDueDate);
END;
GO

IF OBJECT_ID('dbo.MaintenanceJobHistory', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MaintenanceJobHistory
    (
        HistoryId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MaintenanceJobHistory PRIMARY KEY,
        JobId INT NOT NULL,
        ActionDate DATE NOT NULL,
        PerformedBy NVARCHAR(200) NULL,
        Notes NVARCHAR(MAX) NULL,
        Result NVARCHAR(50) NULL,
        NextDueDateCalculated DATE NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_MaintenanceJobHistory_IsDeleted DEFAULT(0),
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_MaintenanceJobHistory_CreatedAt DEFAULT(SYSDATETIME()),
        CONSTRAINT FK_MaintenanceJobHistory_Job FOREIGN KEY (JobId) REFERENCES dbo.MaintenanceJob(JobId)
    );

    CREATE INDEX IX_MaintenanceJobHistory_JobId ON dbo.MaintenanceJobHistory(JobId);
END;
GO

-- Period types
INSERT INTO dbo.GeneralTypes (Category, Name, Code)
SELECT 'PeriodUnit', 'Günlük', 'D' WHERE NOT EXISTS (SELECT 1 FROM dbo.GeneralTypes WHERE Category = 'PeriodUnit' AND Code = 'D');
INSERT INTO dbo.GeneralTypes (Category, Name, Code)
SELECT 'PeriodUnit', 'Haftalık', 'W' WHERE NOT EXISTS (SELECT 1 FROM dbo.GeneralTypes WHERE Category = 'PeriodUnit' AND Code = 'W');
INSERT INTO dbo.GeneralTypes (Category, Name, Code)
SELECT 'PeriodUnit', 'Aylık', 'M' WHERE NOT EXISTS (SELECT 1 FROM dbo.GeneralTypes WHERE Category = 'PeriodUnit' AND Code = 'M');
INSERT INTO dbo.GeneralTypes (Category, Name, Code)
SELECT 'PeriodUnit', 'Yıllık', 'Y' WHERE NOT EXISTS (SELECT 1 FROM dbo.GeneralTypes WHERE Category = 'PeriodUnit' AND Code = 'Y');
INSERT INTO dbo.GeneralTypes (Category, Name, Code)
SELECT 'PeriodUnit', 'Süresiz', 'N' WHERE NOT EXISTS (SELECT 1 FROM dbo.GeneralTypes WHERE Category = 'PeriodUnit' AND Code = 'N');
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceTreeChildren', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceTreeChildren;
GO

CREATE PROCEDURE dbo.sp_MaintenanceTreeChildren
    @ParentId INT = NULL,
    @Search NVARCHAR(200) = NULL,
    @Skip INT = 0,
    @Take INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    IF @ParentId IS NULL
    BEGIN
        SELECT VehicleTypeId AS Id,
               NULL AS ParentId,
               Name AS Text,
               1 AS NodeType,
               CASE WHEN EXISTS (SELECT 1 FROM dbo.Vehicles v WHERE v.MaintenanceVehicleTypeId = vt.VehicleTypeId AND v.IsDeleted = 0) THEN 1 ELSE 0 END AS HasChildren
        FROM dbo.VehicleType vt
        WHERE vt.IsDeleted = 0
          AND (@Search IS NULL OR vt.Name LIKE '%' + @Search + '%' OR vt.Code LIKE '%' + @Search + '%')
        ORDER BY vt.Name
        OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM dbo.VehicleType WHERE VehicleTypeId = @ParentId AND IsDeleted = 0)
    BEGIN
        SELECT v.Id AS Id,
               @ParentId AS ParentId,
               CONCAT(v.Plate, ' ', ISNULL(v.Brand, '')) AS Text,
               2 AS NodeType,
               CASE WHEN EXISTS (SELECT 1 FROM dbo.VehicleComponent c WHERE c.VehicleId = v.Id AND c.IsDeleted = 0) THEN 1 ELSE 0 END AS HasChildren
        FROM dbo.Vehicles v
        WHERE v.MaintenanceVehicleTypeId = @ParentId
          AND v.IsDeleted = 0
          AND (@Search IS NULL OR v.Plate LIKE '%' + @Search + '%' OR v.Brand LIKE '%' + @Search + '%')
        ORDER BY v.Plate
        OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
        RETURN;
    END;

    SELECT vc.VehicleComponentId AS Id,
           vc.VehicleId AS ParentId,
           c.Name AS Text,
           3 AS NodeType,
           0 AS HasChildren
    FROM dbo.VehicleComponent vc
    INNER JOIN dbo.Component c ON c.ComponentId = vc.ComponentId
    WHERE vc.VehicleId = @ParentId
      AND vc.IsDeleted = 0
      AND c.IsDeleted = 0
      AND (@Search IS NULL OR c.Name LIKE '%' + @Search + '%' OR c.Code LIKE '%' + @Search + '%')
    ORDER BY c.Name
    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceTreePath', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceTreePath;
GO

CREATE PROCEDURE dbo.sp_MaintenanceTreePath
    @NodeId INT,
    @NodeType INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @NodeType = 1
    BEGIN
        SELECT VehicleTypeId AS Id, NULL AS ParentId, Name AS Text, 1 AS NodeType FROM dbo.VehicleType WHERE VehicleTypeId = @NodeId;
        RETURN;
    END;

    IF @NodeType = 2
    BEGIN
        SELECT vt.VehicleTypeId AS Id, NULL AS ParentId, vt.Name AS Text, 1 AS NodeType
        FROM dbo.VehicleType vt
        INNER JOIN dbo.Vehicles v ON vt.VehicleTypeId = v.MaintenanceVehicleTypeId
        WHERE v.Id = @NodeId;

        SELECT v.Id AS Id, v.MaintenanceVehicleTypeId AS ParentId, CONCAT(v.Plate, ' ', ISNULL(v.Brand, '')) AS Text, 2 AS NodeType
        FROM dbo.Vehicles v WHERE v.Id = @NodeId;
        RETURN;
    END;

    SELECT vt.VehicleTypeId AS Id, NULL AS ParentId, vt.Name AS Text, 1 AS NodeType
    FROM dbo.VehicleType vt
    INNER JOIN dbo.Vehicles v ON vt.VehicleTypeId = v.MaintenanceVehicleTypeId
    INNER JOIN dbo.VehicleComponent c ON c.VehicleId = v.Id
    WHERE c.VehicleComponentId = @NodeId;

    SELECT v.Id AS Id, v.MaintenanceVehicleTypeId AS ParentId, CONCAT(v.Plate, ' ', ISNULL(v.Brand, '')) AS Text, 2 AS NodeType
    FROM dbo.Vehicles v
    INNER JOIN dbo.VehicleComponent c ON c.VehicleId = v.Id
    WHERE c.VehicleComponentId = @NodeId;

    SELECT c.VehicleComponentId AS Id, c.VehicleId AS ParentId, comp.Name AS Text, 3 AS NodeType
    FROM dbo.VehicleComponent c
    INNER JOIN dbo.Component comp ON comp.ComponentId = c.ComponentId
    WHERE c.VehicleComponentId = @NodeId;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceJobs_List', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceJobs_List;
GO

CREATE PROCEDURE dbo.sp_MaintenanceJobs_List
    @NodeId INT,
    @NodeType INT,
    @Search NVARCHAR(200) = NULL,
    @SortField NVARCHAR(50) = NULL,
    @SortOrder NVARCHAR(4) = 'ASC',
    @Skip INT = 0,
    @Take INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Order NVARCHAR(200) = COALESCE(@SortField, 'NextDueDate');
    DECLARE @OrderBy NVARCHAR(500) = CONCAT(@Order, ' ', CASE WHEN UPPER(@SortOrder) = 'DESC' THEN 'DESC' ELSE 'ASC' END);

    WITH Target AS (
        SELECT * FROM dbo.MaintenanceJob j
        WHERE j.IsDeleted = 0
          AND ((@NodeType = 1 AND j.VehicleTypeId = @NodeId) OR (@NodeType = 2 AND j.VehicleId = @NodeId) OR (@NodeType = 3 AND j.VehicleComponentId = @NodeId))
          AND (@Search IS NULL OR j.JobName LIKE '%' + @Search + '%' OR j.Responsible LIKE '%' + @Search + '%')
    )
    SELECT JobId,
           VehicleTypeId,
           VehicleId,
           VehicleComponentId,
           JobName,
           PeriodUnit,
           PeriodAmount,
           InitialDueDate,
           LastDoneDate,
           NextDueDate,
           Responsible,
           IsActive,
           COUNT(1) OVER() AS TotalCount
    FROM Target
    ORDER BY CASE WHEN @OrderBy LIKE 'JobName %' THEN JobName END,
             CASE WHEN @OrderBy LIKE 'PeriodUnit %' THEN PeriodUnit END,
             CASE WHEN @OrderBy LIKE 'PeriodAmount %' THEN PeriodAmount END,
             CASE WHEN @OrderBy LIKE 'LastDoneDate %' THEN LastDoneDate END,
             CASE WHEN @OrderBy LIKE 'NextDueDate %' THEN NextDueDate END,
             CASE WHEN @OrderBy LIKE 'Responsible %' THEN Responsible END
    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceJobs_Insert', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceJobs_Insert;
GO

CREATE PROCEDURE dbo.sp_MaintenanceJobs_Insert
    @VehicleTypeId INT = NULL,
    @VehicleId INT = NULL,
    @VehicleComponentId INT = NULL,
    @JobName NVARCHAR(300),
    @PeriodUnit CHAR(1),
    @PeriodAmount INT,
    @InitialDueDate DATE = NULL,
    @LastDoneDate DATE = NULL,
    @Responsible NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @NextDueDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MaintenanceJob (VehicleTypeId, VehicleId, VehicleComponentId, JobName, PeriodUnit, PeriodAmount, InitialDueDate, LastDoneDate, NextDueDate, Responsible, IsActive, CreatedAt, UpdatedAt)
    VALUES (@VehicleTypeId, @VehicleId, @VehicleComponentId, @JobName, @PeriodUnit, @PeriodAmount, @InitialDueDate, @LastDoneDate, @NextDueDate, @Responsible, @IsActive, SYSDATETIME(), SYSDATETIME());

    SELECT SCOPE_IDENTITY() AS Id;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceJobs_Update', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceJobs_Update;
GO

CREATE PROCEDURE dbo.sp_MaintenanceJobs_Update
    @JobId INT,
    @JobName NVARCHAR(300),
    @PeriodUnit CHAR(1),
    @PeriodAmount INT,
    @InitialDueDate DATE = NULL,
    @LastDoneDate DATE = NULL,
    @Responsible NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @NextDueDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MaintenanceJob
    SET JobName = @JobName,
        PeriodUnit = @PeriodUnit,
        PeriodAmount = @PeriodAmount,
        InitialDueDate = @InitialDueDate,
        LastDoneDate = @LastDoneDate,
        NextDueDate = @NextDueDate,
        Responsible = @Responsible,
        IsActive = @IsActive,
        UpdatedAt = SYSDATETIME()
    WHERE JobId = @JobId;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceJobs_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceJobs_Delete;
GO

CREATE PROCEDURE dbo.sp_MaintenanceJobs_Delete
    @JobId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MaintenanceJob SET IsDeleted = 1, UpdatedAt = SYSDATETIME() WHERE JobId = @JobId;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceHistory_List', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceHistory_List;
GO

CREATE PROCEDURE dbo.sp_MaintenanceHistory_List
    @JobId INT,
    @Search NVARCHAR(200) = NULL,
    @SortField NVARCHAR(50) = NULL,
    @SortOrder NVARCHAR(4) = 'ASC',
    @Skip INT = 0,
    @Take INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    WITH Target AS (
        SELECT * FROM dbo.MaintenanceJobHistory h
        WHERE h.JobId = @JobId AND h.IsDeleted = 0
          AND (@Search IS NULL OR h.PerformedBy LIKE '%' + @Search + '%' OR h.Notes LIKE '%' + @Search + '%' OR h.Result LIKE '%' + @Search + '%')
    )
    SELECT HistoryId,
           JobId,
           ActionDate,
           PerformedBy,
           Notes,
           Result,
           NextDueDateCalculated,
           COUNT(1) OVER() AS TotalCount
    FROM Target
    ORDER BY CASE WHEN COALESCE(@SortField, '') = 'performedBy' AND UPPER(@SortOrder) = 'DESC' THEN NULL END,
             CASE WHEN COALESCE(@SortField, '') = 'actionDate' AND UPPER(@SortOrder) = 'DESC' THEN NULL END,
             ActionDate DESC
    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
END;
GO

GO
IF OBJECT_ID('dbo.sp_MaintenanceHistory_Add', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_MaintenanceHistory_Add;
GO

CREATE PROCEDURE dbo.sp_MaintenanceHistory_Add
    @JobId INT,
    @ActionDate DATE,
    @PerformedBy NVARCHAR(200) = NULL,
    @Notes NVARCHAR(MAX) = NULL,
    @Result NVARCHAR(50) = NULL,
    @NextDueDateCalculated DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MaintenanceJobHistory (JobId, ActionDate, PerformedBy, Notes, Result, NextDueDateCalculated, CreatedAt)
    VALUES (@JobId, @ActionDate, @PerformedBy, @Notes, @Result, @NextDueDateCalculated, SYSDATETIME());

    SELECT SCOPE_IDENTITY() AS Id;
END;
GO
