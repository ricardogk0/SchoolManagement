using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Entities.Validations;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Domain.Interfaces.Services;
using SchoolManagement.Domain.Interfaces.UoW;
using SchoolManagement.Domain.Notifications;

namespace SchoolManagement.Service.Services;

public class StudentService : BaseService, IStudentService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork, INotifier notifier)
        : base(notifier)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateStudentAsync(StudentEntity student, string userEmail)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var validator = new StudentValidator(_unitOfWork);
            validator.ConfigureRulesForCreate();

            var validationResult = await validator.ValidateAsync(student);
            if (!validationResult.IsValid)
            {
                _notifier.NotifyValidationErrors(validationResult);
                return false;
            }

            student.CreateBy = userEmail;

            await _unitOfWork.Students.Add(student);
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

    public async Task<IEnumerable<StudentEntity>> GetAllAsync()
    {
        try
        {
            return await _unitOfWork.Students.GetAll();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return Enumerable.Empty<StudentEntity>();
        }
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

            student.UpdateBy = userEmail;

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

            student.UpdateBy = userEmail;

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
