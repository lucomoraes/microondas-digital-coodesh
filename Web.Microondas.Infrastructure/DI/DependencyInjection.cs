using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;
using Web.Microondas.Infrastructure.DatabaseContext;
using Web.Microondas.Infrastructure.Persistence;
using Web.Microondas.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IHeatingProgramRepository, HeatingProgramRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}