namespace DeveloperStore.Sales.Application.Queries;

public class BaseQueryRequest
{
    public int? Page { get; set; }
    public int? Size { get; set; }
    public string? Order { get; set; }
    public IDictionary<string, string> Filters { get; set; }
}
