namespace DeveloperStore.Sales.Application.Response;

public class PagedResponse<T>
{
    public T? Data { get; set; }
    public int? TotalItems { get; }
    public int? CurrentPage { get; }
    public int TotalPages { get; }

    public PagedResponse(T data, int? currentPage, int? pageSize, int? totalItems)
    {
        Data = data;
        CurrentPage = currentPage;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)TotalItems.Value/ pageSize.Value); ;
    }
}
