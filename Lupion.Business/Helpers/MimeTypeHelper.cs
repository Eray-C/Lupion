namespace Empty_ERP_Template.Business.Helpers;

public class MimeTypeHelper
{
    public static string GetMimeType(string fileType)
    {
        return fileType.ToLower() switch
        {
            "pdf" => "application/pdf",
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "xlsx" or "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "xls" => "application/vnd.ms-excel",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "doc" => "application/msword",
            _ => "application/octet-stream"
        };
    }
    public static string GetFileExtension(string mimeType)
    {
        return mimeType.ToLower() switch
        {
            // Documents
            "application/pdf" => "pdf",
            "application/msword" => "doc",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "docx",
            "application/vnd.ms-excel" => "xls",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "xlsx",
            "application/vnd.ms-powerpoint" => "ppt",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation" => "pptx",
            "text/plain" => "txt",
            "text/csv" => "csv",
            "application/json" => "json",
            "application/xml" or "text/xml" => "xml",
            "application/rtf" => "rtf",
            "application/vnd.oasis.opendocument.text" => "odt",
            "application/vnd.oasis.opendocument.spreadsheet" => "ods",
            "application/vnd.oasis.opendocument.presentation" => "odp",

            // Images
            "image/jpeg" or "image/jpg" => "jpg",
            "image/png" => "png",
            "image/gif" => "gif",
            "image/bmp" => "bmp",
            "image/webp" => "webp",
            "image/svg+xml" => "svg",
            "image/tiff" => "tiff",

            // Audio
            "audio/mpeg" => "mp3",
            "audio/wav" => "wav",
            "audio/ogg" => "ogg",
            "audio/flac" => "flac",
            "audio/aac" => "aac",

            // Video
            "video/mp4" => "mp4",
            "video/x-msvideo" => "avi",
            "video/x-matroska" => "mkv",
            "video/webm" => "webm",
            "video/quicktime" => "mov",

            // Archives
            "application/zip" => "zip",
            "application/x-7z-compressed" => "7z",
            "application/x-rar-compressed" => "rar",
            "application/gzip" => "gz",
            "application/x-tar" => "tar",

            // Default
            _ => "bin"
        };
    }


}
