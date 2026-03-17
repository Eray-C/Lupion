using Microsoft.AspNetCore.Html;

namespace Empty_ERP_Template.MVC.Models;

public class TwoCardLayout
{
    public string LeftTitle { get; set; }
    public string LeftContentHtml { get; set; }

    public string RightTitle { get; set; }
    public IHtmlContent RightContentHtml { get; set; }

}
