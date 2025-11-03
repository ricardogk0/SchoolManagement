using AutoMapper;
using SchoolManagement.Domain.DTOs.Response;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Service.Mappers;

public class RegistrationMapper : Profile
{
    public RegistrationMapper()
    {
        CreateMap<RegistrationEntity, RegistrationResponseDto>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
            .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Class.Id));
    }
}