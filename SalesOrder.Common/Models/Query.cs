using System.Text.Json;

namespace SalesOrder.Common.Models;

public class Query
{
    public string SearchTerm { get; set; } = null;
    public string SortOrder { get; set; } = "asc";
    public string SortField { get; set; } = "updatedAt";

    public Dictionary<string, string> ToDictionary()
    {
        return JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(this));
    }
}