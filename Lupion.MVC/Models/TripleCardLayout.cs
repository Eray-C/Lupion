using Microsoft.AspNetCore.Html;

namespace Empty_ERP_Template.MVC.Models;

public class TripleCardLayout(
    IHtmlContent leftTitle,
    IHtmlContent leftContent,
    IHtmlContent rightTopTitle,
    IHtmlContent rightTopContent,
    IHtmlContent rightBottomTitle,
    IHtmlContent rightBottomContent)
{
    public IHtmlContent LeftTitle { get; set; } = leftTitle;
    public IHtmlContent LeftContent { get; set; } = leftContent;

    public IHtmlContent RightTopTitle { get; set; } = rightTopTitle;
    public IHtmlContent RightTopContent { get; set; } = rightTopContent;

    public IHtmlContent RightBottomTitle { get; set; } = rightBottomTitle;
    public IHtmlContent RightBottomContent { get; set; } = rightBottomContent;
}
