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

    public override async Task<IQueryable<User>> GetAllAsync()
    {
        return await Task.FromResult(_dbSet
            .Include(u => u.Tasks)
            .Where(u => !u.IsDeleted));
    }

    public async Task<IQueryable<User>> GetByUsernameQueryable(string username)
    {
        return await Task.FromResult(_dbSet
            .Include(u => u.Tasks)
            .Where(u => u.Username == username && !u.IsDeleted));
    }
}

