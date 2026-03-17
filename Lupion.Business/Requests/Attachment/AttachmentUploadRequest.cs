using Microsoft.AspNetCore.Http;

namespace Lupion.Business.Requests.Attachment;
public class AttachmentUploadRequest
{
    public List<IFormFile> Files { get; set; } = new();

    public required string FieldName { get; set; }

    public required string FieldId { get; set; }

    public List<string>? FileNames { get; set; }

    public List<string>? FileTypes { get; set; }

    public List<int>? OriginalSizes { get; set; }

    public List<string>? Comments { get; set; }

    /// <summary>Her dosya iÃ§in Types tablosundan dosya tipi id (Ã¶rn. PersonnelDocumentType).</summary>
    public List<int?>? DocumentTypeIds { get; set; }
}



