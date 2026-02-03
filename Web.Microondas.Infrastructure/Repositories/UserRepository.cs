using Microsoft.EntityFrameworkCore;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Infrastructure.DatabaseContext;

namespace Web.Microondas.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Users?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<IReadOnlyList<Users>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Users.ToListAsync(ct);
    }

    public async Task AddAsync(Users user, CancellationToken ct = default)
    {
        await _context.Users.AddAsync(user, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await GetByIdAsync(id, ct);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public async Task UpdateAsync(Users userToUpdate, Guid id, CancellationToken ct = default)
    {
        var user = await GetByIdAsync(id, ct);
        if (user != null)
        {
            user.Firstname = userToUpdate.Firstname;
            user.Lastname = userToUpdate.Lastname;
            user.Username = userToUpdate.Username;
            user.Password = userToUpdate.Password;
            _context.Users.Update(user);
        }
    }

    public async Task<Users?> GetByUsernameAndPasswordAsync(string username, string passwordHash, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == passwordHash, ct);
    }
}
