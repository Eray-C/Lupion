namespace Empty_ERP_Template.Business.Responses;

public class PagedResponse<T>(IEnumerable<T> data, int totalCount)
{
    public IEnumerable<T> Data { get; set; } = data;
    public int TotalCount { get; set; } = totalCount;
}
