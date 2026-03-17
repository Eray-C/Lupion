-- TrailerDocuments table (dorse belgeleri - araç belgeleri yapısı, kalibrasyon hariç)

IF OBJECT_ID('dbo.TrailerDocuments', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.TrailerDocuments;
END;
GO

CREATE TABLE dbo.TrailerDocuments
(
    Id             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TrailerDocuments PRIMARY KEY,
    IsDeleted      BIT NOT NULL CONSTRAINT DF_TrailerDocuments_IsDeleted DEFAULT(0),
    TrailerId      INT NOT NULL,
    DocumentTypeId INT NOT NULL,
    DocumentNumber NVARCHAR(100) NULL,
    IssueDate      DATE NULL,
    ExpiryDate     DATE NULL,
    Comment        NVARCHAR(MAX) NULL,
    ExtraData      NVARCHAR(MAX) NULL,
    CONSTRAINT FK_TrailerDocuments_Trailer FOREIGN KEY (TrailerId) REFERENCES dbo.tbl_Trailer(TrailerId),
    CONSTRAINT FK_TrailerDocuments_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES dbo.Types(Id)
);
GO

CREATE INDEX IX_TrailerDocuments_TrailerId ON dbo.TrailerDocuments(TrailerId);
CREATE INDEX IX_TrailerDocuments_DocumentTypeId ON dbo.TrailerDocuments(DocumentTypeId);
CREATE INDEX IX_TrailerDocuments_ExpiryDate ON dbo.TrailerDocuments(ExpiryDate);
GO
