using Empty_ERP_Template.Data.Entities.SharedEntities;

namespace Empty_ERP_Template.Data.Entities.AttachmentEntities;
public class Attachments : Entity<int>
{
    public required string FileName { get; set; }
    public string? FileType { get; set; }
    /// <summary>Types tablosundan (örn. PersonnelDocumentType) dosya tipi.</summary>
    public int? DocumentTypeId { get; set; }
    public virtual GeneralType? DocumentType { get; set; }
    public int? OriginalSize { get; set; }
    public string? Comment { get; set; }
    public required string FieldName { get; set; }
    public required string FieldId { get; set; }
    public required string BucketName { get; set; }
    public required string ObjectName { get; set; }
}

