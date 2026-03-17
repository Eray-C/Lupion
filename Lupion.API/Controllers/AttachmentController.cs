using Lupion.Business.Requests.Attachment;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/attachments")]
public class AttachmentsController(AttachmentService attachmentService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] AttachmentUploadRequest request)
    {
        await attachmentService.UploadAsync(request);
        return Ok();
    }
    [HttpGet("{fieldName}/{fieldId}")]
    public async Task<IActionResult> QueryAsync(string fieldName, string fieldId)
    {
        var result = await attachmentService.QueryAsync(fieldName, fieldId);

        return Ok(result);
    }

    [HttpGet("{id}/url")]
    public async Task<IActionResult> GetFileUrl(int id)
    {
        var result = await attachmentService.GetPresignedUrlAsync(id);

        return Ok(new { success = true, url = result.Value.Url, fileName = result.Value.FileName });
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await attachmentService.DeleteAsync(id);

        return Ok();
    }
}
