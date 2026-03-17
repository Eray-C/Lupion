-- Dosya tipi Types tablosundan (PersonnelDocumentType vb.) seçilebilir
IF COL_LENGTH('dbo.Attachments', 'DocumentTypeId') IS NULL
BEGIN
    ALTER TABLE dbo.Attachments ADD DocumentTypeId INT NULL;
    ALTER TABLE dbo.Attachments ADD CONSTRAINT FK_Attachments_DocumentType
        FOREIGN KEY (DocumentTypeId) REFERENCES dbo.Types(Id);
END;
GO

-- Personel modülü dosya tipleri (modül ayarlarından düzenlenebilir)
IF NOT EXISTS (SELECT 1 FROM dbo.Types WHERE Category = 'PersonnelDocumentType')
BEGIN
    INSERT INTO dbo.Types (Category, Name, Code, [Order]) VALUES
        ('PersonnelDocumentType', N'PDF', 'pdf', 1),
        ('PersonnelDocumentType', N'Sözleşme', 'sozlesme', 2),
        ('PersonnelDocumentType', N'CV', 'cv', 3),
        ('PersonnelDocumentType', N'Ehliyet', 'ehliyet', 4),
        ('PersonnelDocumentType', N'Diğer', 'diger', 10);
END;
GO
