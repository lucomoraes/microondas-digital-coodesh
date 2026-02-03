using Microsoft.EntityFrameworkCore;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Infrastructure.Mappings;

namespace Web.Microondas.Infrastructure.DatabaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<HeatingProgram> HeatingPrograms => Set<HeatingProgram>();
    public DbSet<Users> Users => Set<Users>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new HeatinProgramMapping());
        modelBuilder.ApplyConfiguration(new UserMapping());
        DbSeeder.Seed(modelBuilder);
    }
}
