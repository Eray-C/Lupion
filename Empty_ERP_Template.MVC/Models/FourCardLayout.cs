namespace Empty_ERP_Template.MVC.Models;

public class FourCardLayout(
    string leftTopTitle = "",
    string leftTopContentHtml = "",
    string? leftTopHeaderHtml = null,
    string leftBottomTitle = "",
    string leftBottomContentHtml = "",
    string rightTopTitle = "",
    string rightTopContentHtml = "",
    string rightBottomTitle = "",
    string rightBottomContentHtml = "")
{
    public string LeftTopTitle { get; set; } = leftTopTitle;
    public string LeftTopContentHtml { get; set; } = leftTopContentHtml;
    public string? LeftTopHeaderHtml { get; set; } = leftTopHeaderHtml;

    public string LeftBottomTitle { get; set; } = leftBottomTitle;
    public string LeftBottomContentHtml { get; set; } = leftBottomContentHtml;

    public string RightTopTitle { get; set; } = rightTopTitle;
    public string RightTopContentHtml { get; set; } = rightTopContentHtml;

    public string RightBottomTitle { get; set; } = rightBottomTitle;
    public string RightBottomContentHtml { get; set; } = rightBottomContentHtml;
}
