namespace Empty_ERP_Template.Business.Responses;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}

public class BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
