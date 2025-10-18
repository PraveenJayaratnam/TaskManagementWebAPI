namespace Domain.Entities;

[Table("Users", Schema = "TaskManagement")]
public class User : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? FirstName { get; set; }
    
    [StringLength(100)]
    public string? LastName { get; set; }
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
