using AutoMapper;
using SchoolManagement.Domain.DTOs.Response.Common;

namespace SchoolManagement.Service.Converters;

public class GenericPaginationConverter<TSource, TDestination> :
    ITypeConverter<PaginationResponse<TSource>, PaginatedResponseModel<TDestination>>
{
    public PaginatedResponseModel<TDestination> Convert(
        PaginationResponse<TSource> source,
        PaginatedResponseModel<TDestination> destination,
        ResolutionContext context)
    {
        var mapper = context.Mapper;

        var data = mapper.Map<IEnumerable<TDestination>>(source?.Data);

        var metadata = new PaginationMetadata
        {
            PageIndex = source?.PageIndex,
            PageSize = source?.PageSize,
            TotalCount = source?.TotalCount,
            TotalPages = source?.TotalPages
        };

        return PaginatedResponseModel<TDestination>.Success(data, metadata);
    }
}
