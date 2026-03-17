-- Add Color column to Types (GeneralType) for dynamic status/type display (e.g. ShipmentStatusType)
IF COL_LENGTH('dbo.Types', 'Color') IS NULL
BEGIN
    ALTER TABLE dbo.Types ADD Color NVARCHAR(20) NULL;
END;
GO

-- Optional: set default colors for existing ShipmentStatusType rows
UPDATE dbo.Types SET Color = '#facc15' WHERE Category = 'ShipmentStatusType' AND Name = N'Planlandı' AND (Color IS NULL OR Color = '');
UPDATE dbo.Types SET Color = '#22c55e' WHERE Category = 'ShipmentStatusType' AND Name = N'Tamamlandı';
UPDATE dbo.Types SET Color = '#f97316' WHERE Category = 'ShipmentStatusType' AND Name = N'Fatura Edildi';
GO
