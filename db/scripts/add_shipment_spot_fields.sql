-- Spot araç alanları (sadece Shipments tablosu)
IF COL_LENGTH('dbo.Shipments', 'SpotCompanyName') IS NULL
    ALTER TABLE dbo.Shipments ADD SpotCompanyName NVARCHAR(500) NULL;
IF COL_LENGTH('dbo.Shipments', 'InvoiceNumber') IS NULL
    ALTER TABLE dbo.Shipments ADD InvoiceNumber NVARCHAR(100) NULL;
IF COL_LENGTH('dbo.Shipments', 'ExternalDriver') IS NULL
    ALTER TABLE dbo.Shipments ADD ExternalDriver NVARCHAR(500) NULL;
GO
