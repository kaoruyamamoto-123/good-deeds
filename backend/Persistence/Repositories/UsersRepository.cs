using Microsoft.EntityFrameworkCore;

using Domain.Models;
using Application.Users.Interfaces;

namespace Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _dbContext;

    public UsersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }
    
    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Organization)
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
        
        return user;
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        
        return user;
    }

    public async Task<User?> GetByEmailWithRefreshTokens(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .Include(u => u.RefreshTokens)
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        
        return user;   
    }
    
    public async Task<List<User>> GetNonActiveList(CancellationToken cancellationToken = default)
    {
        var nonActiveUsers = await _dbContext.Users
            .Where(u => !u.IsActive && u.CreatedAt.AddHours(1) < DateTime.UtcNow)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
            
        return nonActiveUsers;
    }

    public void Update(User user) => _dbContext.Update(user);
    
    public void DeleteRange(IEnumerable<User> users) =>
        _dbContext.Users.RemoveRange(users);
}