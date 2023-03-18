namespace SalesOrder.Common.Models;

public class ApiResponse<TResult>
{
    public bool Success { get; set; }
    public TResult Result { get; set; }
    public string Message { get; set; }
}

public class ApiPaginatedResponse<TResult>
{
    public bool Success { get; set; }
    public PaginatedList<TResult> Result { get; set; }
    public string Message { get; set; }
}