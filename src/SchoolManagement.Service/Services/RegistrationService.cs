using AutoMapper;
using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.DTOs.Response;
using SchoolManagement.Domain.DTOs.Response.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Entities.Validations;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Domain.Interfaces.Services;
using SchoolManagement.Domain.Interfaces.UoW;
using SchoolManagement.Service.Resources;
using System.Net;

namespace SchoolManagement.Service.Services;

public class RegistrationService : BaseService, IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ValidationErrorMessages _validationMessages;

    public RegistrationService(IUnitOfWork unitOfWork, INotifier notifier, IMapper mapper, ValidationErrorMessages validationMessages) : base(notifier)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationMessages = validationMessages;
    }

    public async Task<ResponseModel<RegistrationResponseDto>> RegisterStudentAsync(RegistrationCreateDto registrationDto)
    {
        try
        {
            var student = await _unitOfWork.Students.GetById(registrationDto.StudentId);
            var @class = await _unitOfWork.Classes.GetById(registrationDto.ClassId);

            if (student == null || @class == null)
            {
                var error = new ErrorInfo(System.Net.HttpStatusCode.BadRequest, "Aluno ou turma não encontrados");
                return ResponseModel<RegistrationResponseDto>.Failure(error);
            }

            await _unitOfWork.BeginTransactionAsync();
            var entity = new RegistrationEntity
            {
                Student = student,
                Class = @class
            };

            var validator = new RegistrationValidator(_unitOfWork);
            validator.ConfigureRulesForCreate();

            var validationResult = await validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                _notifier.NotifyValidationErrors(validationResult);
                var errors = _notifier.GetNotifications()
                    .Select(n => new ErrorInfo { Message = n.Message, StatusCode = HttpStatusCode.BadRequest, Details = n.Type.ToString() }).Where(e => e.Details == "Error")
                    .FirstOrDefault();
                return ResponseModel<RegistrationResponseDto>.Failure(errors);
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "ricardokevi@gmail.com";
            await _unitOfWork.Registrations.AddRegistrationStudent(entity);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();
            var response = _mapper.Map<RegistrationResponseDto>(entity);
            return ResponseModel<RegistrationResponseDto>.Success(response);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return ResponseModel<RegistrationResponseDto>.Failure(_validationMessages.ErrorCreateRegistration);
        }
    }

    public async Task<ResponseModel<IEnumerable<StudentResponseDto>>> GetStudentsByClassAsync(Guid classId)
    {
        try
        {
            var students = await _unitOfWork.Registrations.GetStudentsByClass(classId);
            var response = _mapper.Map<IEnumerable<StudentResponseDto>>(students);
            return ResponseModel<IEnumerable<StudentResponseDto>>.Success(response);
        }
        catch (Exception)
        {
            var error = new ErrorInfo(System.Net.HttpStatusCode.BadRequest, "Erro ao listar alunos da turma");
            return ResponseModel<IEnumerable<StudentResponseDto>>.Failure(error);
        }
    }
}
