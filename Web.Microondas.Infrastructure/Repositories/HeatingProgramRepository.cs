using Microsoft.EntityFrameworkCore;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Infrastructure.DatabaseContext;

namespace Web.Microondas.Infrastructure.Repositories;

public class HeatingProgramRepository : IHeatingProgramRepository
{
    private readonly ApplicationDbContext _context;

    public HeatingProgramRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HeatingProgram?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.HeatingPrograms
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<HeatingProgram>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.HeatingPrograms
            .AsNoTracking()
            .OrderBy(x => x.IsPreset ? 0 : 1)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<bool> CheckIfExistsAsync(char character, CancellationToken ct = default)
    {
        return await _context.HeatingPrograms
            .AnyAsync(x => x.Character == character, ct);
    }

    public async Task AddAsync(HeatingProgram program, CancellationToken ct = default)
    {
        await _context.HeatingPrograms.AddAsync(program, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var program = await GetByIdAsync(id, ct);

        if (program != null && !program.IsPreset)
        {
            _context.HeatingPrograms.Remove(program);
        }
    }
}