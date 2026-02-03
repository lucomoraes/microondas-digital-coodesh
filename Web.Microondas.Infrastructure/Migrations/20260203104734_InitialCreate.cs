using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Web.Microondas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatingPrograms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    food = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    time_in_seconds = table.Column<int>(type: "int", nullable: false),
                    power = table.Column<int>(type: "int", nullable: false),
                    character = table.Column<string>(type: "char(1)", fixedLength: true, nullable: false),
                    instructions = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    is_preset = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingPrograms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "HeatingPrograms",
                columns: new[] { "id", "character", "created_at", "food", "instructions", "is_preset", "name", "power", "time_in_seconds" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), "*", new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Pipoca (de micro-ondas)", "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento.", true, "Pipoca", 7, 180 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "#", new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Leite", "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras.", true, "Leite", 5, 300 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "@", new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Carne em pedaço ou fatias", "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme.", true, "Carnes de boi", 4, 840 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "&", new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Frango (qualquer corte)", "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme.", true, "Frango", 7, 480 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "%", new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Feijão congelado", "Deixe o recipiente destampado e, em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas.", true, "Feijão", 9, 480 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "created_at", "firstname", "lastname", "password_hash", "username" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Admin", "User", "d033e22ae348aeb5660fc2140aec35850c4da997", "ADM" });

            migrationBuilder.CreateIndex(
                name: "IX_HeatingPrograms_character",
                table: "HeatingPrograms",
                column: "character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_username",
                table: "Users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatingPrograms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
