using AutoMapper;
using SchoolManagement.Domain.Common;
using System.Diagnostics.CodeAnalysis;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Dtos.Response;

namespace SchoolManagement.Service.Mappers;

[ExcludeFromCodeCoverage]
public class StudentMapper : Profile
{
    public StudentMapper()
    {
        CreateMap<StudentCreateDto, StudentEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        CreateMap<StudentEntity, StudentResponseDto>();

        CreateMap<PaginatedResponse<StudentEntity>, PaginatedResponse<StudentResponseDto>>()
            .ForMember(dest => dest.Items,
               opt => opt.MapFrom(src => src.Items
                   .OrderBy(s => s.Name)));

        CreateMap<IEnumerable<StudentEntity>, PaginatedResponse<StudentResponseDto>>()
            .ConvertUsing((src, dest, context) =>
            {
                var studentDtos = context.Mapper.Map<List<StudentResponseDto>>(src)
                    .OrderBy(s => s.Name)
                    .ToList();
                return new PaginatedResponse<StudentResponseDto>(
                    studentDtos, 
                    studentDtos.Count, 
                    1, 
                    studentDtos.Count 
                );
            });
    }
}
