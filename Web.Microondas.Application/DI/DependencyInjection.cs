using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Web.Microondas.Application.Helpers;
using Web.Microondas.Application.Services.Implementations;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.Auth.Handlers;
using Web.Microondas.Application.UseCases.Auth.Handlers.Interfaces;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;
using Web.Microondas.Application.UseCases.User.Handlers;
using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

namespace Web.Microondas.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoginHandler, LoginHandler>();
        services.AddScoped<IJwtHelper, JwtHelper>();

        services.AddSingleton<IMicrowaveService, MicrowaveService>();
        services.AddScoped<IHeatingProgramService, HeatingProgramService>();
        services.AddScoped<ICreateHeatingProgramHandler, CreateHeatingProgramHandler>();
        services.AddScoped<IDeleteHeatingProgramHandler, DeleteHeatingProgramHandler>();
        services.AddScoped<IGetAllHeatingProgramsHandler, GetAllHeatingProgramsHandler>();
        services.AddScoped<IGetHeatingProgramByIdHandler, GetHeatingProgramByIdHandler>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICreateUserHandler, CreateUserHandler>();
        services.AddScoped<IUpdateUserHandler, UpdateUserHandler>();
        services.AddScoped<IDeleteUserHandler, DeleteUserHandler>();
        services.AddScoped<IGetUserByIdHandler, GetUserByIdHandler>();
        services.AddScoped<IGetAllUsersHandler, GetAllUsersHandler>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}