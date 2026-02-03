using Microsoft.EntityFrameworkCore;
using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Infrastructure.DatabaseContext;

public static class DbSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var admUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var staticDate = new DateTime(2024, 2, 3, 12, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Users>().HasData(new Users
        {
            Id = admUserId,
            Username = "ADM",
            Firstname = "Admin",
            Lastname = "User",
            Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", // "admin" (SHA-256)
            CreatedAt = staticDate
        });

        modelBuilder.Entity<HeatingProgram>().HasData(
            new HeatingProgram
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Pipoca",
                Food = "Pipoca (de micro-ondas)",
                TimeInSeconds = 180,
                Power = 7,
                Character = '*',
                Instructions = "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento.",
                IsPreset = true,
                CreatedAt = staticDate
            },
            new HeatingProgram
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Leite",
                Food = "Leite",
                TimeInSeconds = 300,
                Power = 5,
                Character = '#',
                Instructions = "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras.",
                IsPreset = true,
                CreatedAt = staticDate
            },
            new HeatingProgram
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Carnes de boi",
                Food = "Carne em pedaço ou fatias",
                TimeInSeconds = 840,
                Power = 4,
                Character = '@',
                Instructions = "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme.",
                IsPreset = true,
                CreatedAt = staticDate
            },
            new HeatingProgram
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Frango",
                Food = "Frango (qualquer corte)",
                TimeInSeconds = 480,
                Power = 7,
                Character = '&',
                Instructions = "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme.",
                IsPreset = true,
                CreatedAt = staticDate
            },
            new HeatingProgram
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Name = "Feijão",
                Food = "Feijão congelado",
                TimeInSeconds = 480,
                Power = 9,
                Character = '%',
                Instructions = "Deixe o recipiente destampado e, em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas.",
                IsPreset = true,
                CreatedAt = staticDate
            }
        );
    }
}