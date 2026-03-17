using Microsoft.AspNetCore.Mvc;

namespace Lupion.MVC.Controllers
{
    public class AttachmentController : BaseController
    {
        public PartialViewResult LayoutAttachment()
        {
            return PartialView();
        }
        public PartialViewResult LayoutAttachmentUploader(string documentTypeCategory = null)
        {
            ViewBag.DocumentTypeCategory = documentTypeCategory;
            return PartialView();
        }
        public PartialViewResult LayoutAttachmentViewer()
        {
            return PartialView();
        }
    }
}
