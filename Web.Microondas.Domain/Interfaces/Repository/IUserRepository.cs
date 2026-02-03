using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Domain.Interfaces.Repository;

public interface IUserRepository : IRepository<Users, Guid>
{   
    Task UpdateAsync(Users userToUpdate, Guid id, CancellationToken ct = default);
    Task<Users?> GetByUsernameAndPasswordAsync(string username, string passwordHash, CancellationToken ct = default);
}
