-- Hazır fiyatlara çıkış/varış adresleri, yükleme/teslimat firma ve il/ilçe alanları
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('CustomerPrices') AND name = 'DepartureRegion')
BEGIN
    ALTER TABLE CustomerPrices ADD DepartureRegion NVARCHAR(500) NULL;
    ALTER TABLE CustomerPrices ADD ArrivalRegion NVARCHAR(500) NULL;
    ALTER TABLE CustomerPrices ADD DepartureCompany NVARCHAR(500) NULL;
    ALTER TABLE CustomerPrices ADD ArrivalCompany NVARCHAR(500) NULL;
    ALTER TABLE CustomerPrices ADD DepartureCityId INT NULL;
    ALTER TABLE CustomerPrices ADD DepartureTownId INT NULL;
    ALTER TABLE CustomerPrices ADD ArrivalCityId INT NULL;
    ALTER TABLE CustomerPrices ADD ArrivalTownId INT NULL;
END
GO
