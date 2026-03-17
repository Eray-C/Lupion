using Microsoft.AspNetCore.Http;

namespace Empty_ERP_Template.Business.Requests.Attachment;
public class AttachmentUploadRequest
{
    public List<IFormFile> Files { get; set; } = new();

    public required string FieldName { get; set; }

    public required string FieldId { get; set; }

    public List<string>? FileNames { get; set; }

    public List<string>? FileTypes { get; set; }

    public List<int>? OriginalSizes { get; set; }

    public List<string>? Comments { get; set; }

    /// <summary>Her dosya için Types tablosundan dosya tipi id (örn. PersonnelDocumentType).</summary>
    public List<int?>? DocumentTypeIds { get; set; }
}



