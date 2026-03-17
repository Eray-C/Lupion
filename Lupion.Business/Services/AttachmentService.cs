using AutoMapper;
using Lupion.Business.DTOs.Attachment;
using Lupion.Business.Exceptions;
using Lupion.Business.Requests.Attachment;
using Lupion.Business.Services;
using Lupion.Data;
using Lupion.Data.Entities.AttachmentEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class AttachmentService(DBContext context, MinioService minioService, IMapper mapper, IHttpContextAccessor httpContextAccessor) : BaseService(httpContextAccessor)
{

    public async Task UploadAsync(AttachmentUploadRequest request)
    {
        var entities = mapper.Map<IEnumerable<Attachments>>(request);

        int index = 0;
        foreach (var entity in entities)
        {
            var file = request.Files[index];
            entity.ObjectName = $"{CurrentUser.CompanyCode}/{entity.FieldName}/{entity.FieldId}/{file.FileName}";

            using var stream = file.OpenReadStream();
            await minioService.UploadStreamAsync(
                entity.BucketName,
                entity.ObjectName,
                stream,
                file.Length,
                file.ContentType ?? "application/octet-stream"
            );

            context.Attachments.Add(entity);
            index++;
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<AttachmentDTO>> QueryAsync(string fieldName, string fieldId)
    {
        var attachments = await context.Attachments
            .Where(x => x.FieldName == fieldName && x.FieldId == fieldId)
            .Include(x => x.DocumentType)
            .ToListAsync();

        return mapper.Map<List<AttachmentDTO>>(attachments);
    }


    public async Task<(string Url, string FileName)?> GetPresignedUrlAsync(int id, int expirySeconds = 3600)
    {
        var entity = await context.Attachments.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return null;

        var url = await minioService.GetFileUrlAsync(entity.BucketName, entity.ObjectName, expirySeconds);

        return (url, entity.FileName);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await context.Attachments.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }
}
