using SchoolManagement.Domain.DTOs.Response.Common;

namespace SchoolManagement.Domain.Common;

public class Filters : BasePagination
{
    public string? studentName { get; set; }
    public string? studentDocument { get; set; }
}
