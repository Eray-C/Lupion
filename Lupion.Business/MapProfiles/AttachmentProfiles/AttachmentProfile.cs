using AutoMapper;
using Lupion.Business.DTOs.Attachment;
using Lupion.Business.Helpers;
using Lupion.Business.Requests.Attachment;
using Lupion.Data.Entities.AttachmentEntities;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<AttachmentUploadRequest, IEnumerable<Attachments>>()
            .ConvertUsing((src, dest, context) =>
            {
                var list = new List<Attachments>();

                for (int i = 0; i < src.Files.Count; i++)
                {
                    var file = src.Files[i];

                    var entity = new Attachments
                    {
                        FileName = src.FileNames?.ElementAtOrDefault(i) ?? file.FileName,
                        FileType = MimeTypeHelper.GetFileExtension(file.ContentType),
                        DocumentTypeId = src.DocumentTypeIds?.ElementAtOrDefault(i),
                        OriginalSize = src.OriginalSizes?.ElementAtOrDefault(i) ?? (int)file.Length,
                        Comment = src.Comments?.ElementAtOrDefault(i),
                        FieldName = src.FieldName,
                        FieldId = src.FieldId,
                        BucketName = "cheetah-files",
                        ObjectName = string.Empty
                    };

                    list.Add(entity);
                }

                return list;
            });

        CreateMap<Attachments, AttachmentDTO>()
            .ForMember(d => d.DocumentTypeName, o => o.MapFrom(s => s.DocumentType != null ? s.DocumentType.Name : s.FileType));
    }
}
