namespace SchoolManagement.Domain.DTOs.Response.Common;

public class PaginationResponse<TModel> : BasePagination
{
    public IEnumerable<TModel> Data { get; set; } = [];
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public PaginationResponse(
        IEnumerable<TModel> data,
        int count,
        int pageNumber,
        int pageSize)
    {
        Data = data;
        TotalCount = count;
        TotalPages = CalculateTotalPages(count, pageSize);
        PageIndex = pageNumber;
        PageSize = pageSize;
    }

    private int CalculateTotalPages(long count, int pageSize)
    {
        if (pageSize == 0) return 0;

        return (int)Math.Ceiling((double)count / pageSize);
    }
}
