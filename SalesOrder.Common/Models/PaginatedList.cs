namespace SalesOrder.Common.Models;

public class PaginatedList<T>
{
    public int Count { get; set; }
    public IEnumerable<T> Items { get; set; } = new List<T>();
}