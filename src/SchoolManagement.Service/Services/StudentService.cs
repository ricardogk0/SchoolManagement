using AutoMapper;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Dtos.Request;
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

public class StudentService : BaseService, IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ValidationErrorMessages _validationMessages;

    public StudentService(IUnitOfWork unitOfWork, INotifier notifier, IMapper mapper, ValidationErrorMessages validationErrorMessages)
        : base(notifier)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationMessages = validationErrorMessages;
    }

    public async Task<ResponseModel<StudentResponseDto>> CreateStudentAsync(StudentCreateDto student, string userEmail)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var validator = new StudentValidator(_unitOfWork);
            validator.ConfigureRulesForCreate();

            var studentEntity = _mapper.Map<StudentEntity>(student);
            studentEntity.CreatedBy = userEmail;

            var validationResult = await validator.ValidateAsync(studentEntity);
            if (!validationResult.IsValid)
            {
                _notifier.NotifyValidationErrors(validationResult);
                var errors = _notifier.GetNotifications()
                    .Select(n => new ErrorInfo { Message = n.Message, StatusCode = HttpStatusCode.BadRequest })
                    .FirstOrDefault();
                return ResponseModel<StudentResponseDto>.Failure(errors);
            }

            await _unitOfWork.Students.Add(studentEntity);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            var response = _mapper.Map<StudentResponseDto>(studentEntity);

            return ResponseModel<StudentResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            HandleException(ex);
            return ResponseModel<StudentResponseDto>.Failure(_validationMessages.ErrorCreateStudent);
        }
    }

    public async Task<PaginatedResponse<StudentResponseDto>> GetAllAsync()
    {
        var students = await _unitOfWork.Students.GetAll();
        return _mapper.Map<PaginatedResponse<StudentResponseDto>>(students);
    }

    public async Task<StudentEntity?> GetByIdAsync(Guid studentId)
    {
        try
        {
            return await _unitOfWork.Students.GetById(studentId);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return null;
        }
    }

    public async Task<bool> SoftDeleteStudentAsync(Guid studentId, string userEmail)
    {
        try
        {
            var student = await _unitOfWork.Students.GetById(studentId);
            if (student == null)
            {
                _notifier.Handle("Student not found.");
                return false;
            }

            student.UpdatedBy = userEmail;

            student.ToggleIsDeleted();
            await _unitOfWork.Students.Update(student);
            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return false;
        }
    }

    public async Task<bool> UpdateStudentAsync(StudentEntity student, string userEmail)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingStudent = await _unitOfWork.Students.GetById(student.Id);
            if (existingStudent == null)
            {
                _notifier.Handle("Student not found", NotificationType.Error);
                return false;
            }

            var validator = new StudentValidator(_unitOfWork);
            validator.ConfigureRulesForUpdate(existingStudent);

            var validationResult = await validator.ValidateAsync(student);
            if (!validationResult.IsValid)
            {
                _notifier.NotifyValidationErrors(validationResult);
                return false;
            }

            student.UpdatedBy = userEmail;

            await _unitOfWork.Students.Update(student);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            HandleException(ex);
            return false;
        }
    }
}
