namespace SchoolManagement.Domain.DTOs.Response.Common;

public abstract class BasePagination
{
    private const int MAX_PAGE_SIZE = 50;
    private const int DEFAULT_PAGE_SIZE = 10;

    private int _pageSize = DEFAULT_PAGE_SIZE;
    private int _pageIndex = 1;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value > 0 ? value : 1;
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = SetPageSize(value);
    }

    private static int SetPageSize(int value)
    {
        if (value <= 0) return DEFAULT_PAGE_SIZE;
        else if (value > MAX_PAGE_SIZE) return MAX_PAGE_SIZE;

        return value;
    }
}
