using AutoMapper;
using SchoolManagement.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace SchoolManagement.Service.Mappers;

[ExcludeFromCodeCoverage]
public class StudentMapper : Profile
{
    public StudentMapper()
    {
        CreateMap<Domain.Dtos.Request.StudentCreateDto, Domain.Entities.StudentEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        CreateMap<Domain.Entities.StudentEntity, Domain.Dtos.Response.StudentResponseDto>();

        CreateMap<IEnumerable<Domain.Entities.StudentEntity>, PaginatedResponse<Domain.Dtos.Response.StudentResponseDto>>()
            .ConvertUsing((src, dest, context) =>
            {
                var studentDtos = context.Mapper.Map<List<Domain.Dtos.Response.StudentResponseDto>>(src);
                return new PaginatedResponse<Domain.Dtos.Response.StudentResponseDto>(
                    studentDtos, 
                    studentDtos.Count, 
                    1, 
                    studentDtos.Count 
                );
            });
    }
}
