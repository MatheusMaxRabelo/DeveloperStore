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
        TotalPages = (int)Math.Ceiling(TotalItems ?? 0 / (double) pageSize); ;
        TotalItems = totalItems;
    }
}
