using AutoMapper;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Service.Mappers;

public class ClassMapper : Profile
{
    public ClassMapper()
    {
        CreateMap<ClassCreateDto, ClassEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<ClassEntity, ClassResponseDto>();

        CreateMap<IEnumerable<ClassEntity>, PaginatedResponse<ClassResponseDto>>()
            .ConvertUsing((src, dest, context) =>
            {
                var dtos = context.Mapper.Map<List<ClassResponseDto>>(src)
                        .OrderBy(c => c.ClassName)
                        .ToList();
                return new PaginatedResponse<ClassResponseDto>(
                    dtos,
                    dtos.Count,
                    1,
                    dtos.Count
                );
            });
    }
}