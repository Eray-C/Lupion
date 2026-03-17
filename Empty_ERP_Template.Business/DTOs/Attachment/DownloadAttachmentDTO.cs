namespace Empty_ERP_Template.Business.DTOs.Attachment;
public class DownloadAttachmentDTO
{
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = "application/octet-stream";
    public byte[] Data { get; set; } = Array.Empty<byte>();
}
