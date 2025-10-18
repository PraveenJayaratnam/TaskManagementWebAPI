using Infrastructure.Data;
using Domain.Entities;
using Application.Services;

namespace Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context, IAuditService auditService) : base(context, auditService)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Where(u => u.Username == username && !u.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _dbSet
            .Where(u => u.Username == username && !u.IsDeleted)
            .AnyAsync();
    }

    public override async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.Tasks)
            .Where(u => u.Id == id && !u.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.Tasks)
            .Where(u => !u.IsDeleted)
            .ToListAsync();
    }
}

