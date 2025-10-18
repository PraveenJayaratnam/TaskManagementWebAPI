using Domain.Enums;

namespace Domain.Entities;

[Table("TaskItems", Schema = "TaskManagement")]
public class TaskItem : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    
    public TaskPriority? Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
