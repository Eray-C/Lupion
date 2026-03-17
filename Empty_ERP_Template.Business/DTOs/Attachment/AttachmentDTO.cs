namespace Empty_ERP_Template.Business.DTOs.Attachment;
public class AttachmentDTO
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string? FileType { get; set; }
    public int? DocumentTypeId { get; set; }
    public string? DocumentTypeName { get; set; }
    public int? OriginalSize { get; set; }
    public string? Comment { get; set; }
    public string FieldName { get; set; }
    public string FieldId { get; set; }
    public string BucketName { get; set; }
    public string ObjectName { get; set; }
}

