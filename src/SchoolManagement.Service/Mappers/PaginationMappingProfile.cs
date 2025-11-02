using AutoMapper;
using SchoolManagement.Domain.DTOs.Response.Common;
using SchoolManagement.Service.Converters;

namespace SchoolManagement.Service.Mappers;

public class PaginationMappingProfile : Profile
{
    public PaginationMappingProfile()
    {
        CreateMap(typeof(PaginationResponse<>), typeof(PaginatedResponseModel<>))
            .ConvertUsing(typeof(GenericPaginationConverter<,>));
    }
}
