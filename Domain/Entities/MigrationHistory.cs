namespace Domain.Entities;

[Table("MigrationHistory", Schema = "TaskManagement")]
public class MigrationHistory : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string MigrationName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Version { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime AppliedAt { get; set; }
    
    [StringLength(100)]
    public string? AppliedBy { get; set; }
    
    [StringLength(50)]
    public string? Environment { get; set; }
    
    [StringLength(1000)]
    public string? RollbackScript { get; set; }
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    public bool IsRolledBack { get; set; } = false;
    
    [StringLength(100)]
    public string? RolledBackBy { get; set; }
    
    public DateTime? RolledBackAt { get; set; }
}
