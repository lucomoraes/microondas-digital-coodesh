using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Domain.Interfaces.Repository;

public interface IHeatingProgramRepository : IRepository<HeatingProgram, Guid>
{   
    Task<bool> CheckIfExistsAsync(char character, CancellationToken ct = default);
}
