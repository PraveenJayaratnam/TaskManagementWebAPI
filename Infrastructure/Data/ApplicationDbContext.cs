using Domain.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    public new DbSet<T> Set<T>() where T : BaseEntity
    {
        return base.Set<T>();
    }

    public new void Add<T>(T entity) where T : BaseEntity
    {
        base.Add(entity);
    }

    public new void Update<T>(T entity) where T : BaseEntity
    {
        base.Update(entity);
    }

    public new void Remove<T>(T entity) where T : BaseEntity
    {
        base.Remove(entity);
    }

    public async Task<T?> FindAsync<T>(Guid id) where T : BaseEntity
    {
        return await Set<T>().FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseEntity
    {
        return await Set<T>().Where(e => !e.IsDeleted).ToListAsync();
    }

    public async Task<bool> ExistsAsync<T>(Guid id) where T : BaseEntity
    {
        return await Set<T>().AnyAsync(e => e.Id == id && !e.IsDeleted);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureBaseEntityProperties(modelBuilder);
        ConfigureNavigationProperties(modelBuilder);

        DbSets.ConfigureDbSets(modelBuilder);

        DbSets.SeedData(modelBuilder);
    }

    private void ConfigureBaseEntityProperties(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.IsActive))
                    .HasDefaultValue(true);
                
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.IsDeleted))
                    .HasDefaultValue(false);
            }
        }
    }

    private void ConfigureNavigationProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.UpdatedBy)
            .WithMany()
            .HasForeignKey(t => t.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.CreatedBy)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UpdatedBy)
            .WithMany()
            .HasForeignKey(u => u.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }

}

