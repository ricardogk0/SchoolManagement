using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.DTOs.Response;
using SchoolManagement.Domain.DTOs.Response.Common;

namespace SchoolManagement.Domain.Interfaces.Services;

public interface IRegistrationService
{
    Task<ResponseModel<RegistrationResponseDto>> RegisterStudentAsync(RegistrationCreateDto registrationDto);
    Task<ResponseModel<IEnumerable<StudentResponseDto>>> GetStudentsByClassAsync(Guid classId);
}
