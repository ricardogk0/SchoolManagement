using AutoMapper;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.DTOs.Response.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Entities.Validations;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Domain.Interfaces.Services;
using SchoolManagement.Domain.Interfaces.UoW;
using SchoolManagement.Domain.Notifications;
using SchoolManagement.Service.Resources;
using System.Net;

namespace SchoolManagement.Service.Services;

public class ClassService : BaseService, IClassService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ValidationErrorMessages _validationMessages;

    public ClassService(IUnitOfWork unitOfWork, INotifier notifier, IMapper mapper, ValidationErrorMessages validationErrorMessages)
        : base(notifier)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationMessages = validationErrorMessages;
    }

    public async Task<PaginatedResponse<ClassResponseDto>> GetAllAsync()
    {
        var classes = await _unitOfWork.Classes.GetAll();
        var dtos = _mapper.Map<List<ClassResponseDto>>(classes);

        foreach (var dto in dtos)
        {
            dto.StudentCount = await _unitOfWork.Registrations.GetStudentCountByClass(dto.Id);
        }

        var ordered = dtos.OrderBy(c => c.ClassName).ToList();
        return new PaginatedResponse<ClassResponseDto>(ordered, ordered.Count, 1, ordered.Count);
    }

    public async Task<ClassResponseDto?> GetByIdAsync(Guid classId)
    {
        try
        {
            var classe = await _unitOfWork.Classes.GetById(classId);
            var dto = _mapper.Map<ClassResponseDto>(classe);
            if (dto != null)
            {
                dto.StudentCount = await _unitOfWork.Registrations.GetStudentCountByClass(dto.Id);
            }
            return dto;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return null;
        }
    }

    public async Task<ResponseModel<ClassResponseDto>> CreateClassAsync(ClassCreateDto classe, string userEmail)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var validator = new ClassValidator();
            validator.ConfigureRulesForCreate();

            var classEntity = _mapper.Map<ClassEntity>(classe);
            classEntity.CreatedBy = userEmail;

            var validationResult = await validator.ValidateAsync(classEntity);
            if (!validationResult.IsValid)
            {
                _notifier.NotifyValidationErrors(validationResult);
                var errors = _notifier.GetNotifications()
                    .Select(n => new ErrorInfo { Message = n.Message, StatusCode = HttpStatusCode.BadRequest, Details = n.Type.ToString() }).Where(e => e.Details == "Error")
                    .FirstOrDefault();
                return ResponseModel<ClassResponseDto>.Failure(errors);
            }

            await _unitOfWork.Classes.Add(classEntity);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            var response = _mapper.Map<ClassResponseDto>(classEntity);
            response.StudentCount = 0;

            return ResponseModel<ClassResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            HandleException(ex);
            return ResponseModel<ClassResponseDto>.Failure(_validationMessages.ErrorCreateClass);
        }
    }

    public async Task<ClassResponseDto?> UpdateClassAsync(Guid classId, ClassUpdateDto classe, string userEmail)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingClass = await _unitOfWork.Classes.GetById(classId);
            if (existingClass == null)
            {
                _notifier.Handle("Class not found", NotificationType.Error);
                return null;
            }

            existingClass.ClassName = classe.ClassName ?? existingClass.ClassName;
            existingClass.Description = classe.Description ?? existingClass.Description;

            var validator = new ClassValidator();
            validator.ConfigureRulesForUpdate(existingClass);

            var validationResult = await validator.ValidateAsync(existingClass);
            if (!validationResult.IsValid)
            {
                _notifier.NotifyValidationErrors(validationResult);
                await _unitOfWork.RollbackTransactionAsync();
                return null;
            }

            existingClass.UpdatedBy = userEmail;
            existingClass.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Classes.Update(existingClass);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            var dto = _mapper.Map<ClassResponseDto>(existingClass);
            dto.StudentCount = await _unitOfWork.Registrations.GetStudentCountByClass(dto.Id);
            return dto;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            HandleException(ex);
            return null;
        }
    }

    public async Task<bool> SoftDeleteClassAsync(Guid classId, string userEmail)
    {
        try
        {
            var classe = await _unitOfWork.Classes.GetById(classId);
            if (classe == null)
            {
                _notifier.Handle("Class not found.");
                return false;
            }

            classe.UpdatedBy = userEmail;
            classe.ToggleIsDeleted();

            await _unitOfWork.Classes.Update(classe);
            await _unitOfWork.SaveAsync();
            return true;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return false;
        }
    }
}
