namespace SalesOrder.Common.Models;

public class PaginationQuery : Query
{
    public int PageSize { get; set; } = 10;
    public int Page { get; set; } = 1;
}

public class WindowPaginationQuery : PaginationQuery
{
    public int? OrderId { get; set; }
}

public class SubElementPaginationQuery : PaginationQuery
{
    public int? OrderId { get; set; }

    public int? WindowId { get; set; }
}