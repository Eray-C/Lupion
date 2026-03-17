namespace Empty_ERP_Template.Business.Requests.Attachment;
public class AttachmentRequest
{
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public string? CompressedData { get; set; }
    public int? OriginalSize { get; set; }
    public string? Comment { get; set; }
    public string? FieldName { get; set; }
    public string? FieldId { get; set; }
}

