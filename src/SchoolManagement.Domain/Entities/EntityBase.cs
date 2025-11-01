namespace SchoolManagement.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreateBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdateBy { get; set; }
    public bool IsDeleted { get; set; }

    public EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void ToggleIsDeleted()
    {
        Update();
        IsDeleted = !IsDeleted;
    }
}
