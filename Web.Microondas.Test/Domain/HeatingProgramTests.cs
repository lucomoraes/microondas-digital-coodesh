using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Test.Domain;

public class HeatingProgramTests
{
    [Fact]
    public void CreatePresetHeating_ShouldCreateWithCorrectProperties()
    {
        // Arrange & Act
        var program = HeatingProgram.CreatePresetHeating(
            "Pipoca", "Pipoca (de micro-ondas)", 180, 7, '*', "Instruções de teste");

        // Assert
        program.Name.Should().Be("Pipoca");
        program.Food.Should().Be("Pipoca (de micro-ondas)");
        program.TimeInSeconds.Should().Be(180);
        program.Power.Should().Be(7);
        program.Character.Should().Be('*');
        program.Instructions.Should().Be("Instruções de teste");
        program.IsPreset.Should().BeTrue();
        program.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateCustomHeating_ShouldCreateWithCorrectProperties()
    {
        // Arrange & Act
        var program = HeatingProgram.CreateCustomHeating(
            "Meu Programa", "Meu Alimento", 60, 5, '~', "Minhas instruções");

        // Assert
        program.Name.Should().Be("Meu Programa");
        program.Food.Should().Be("Meu Alimento");
        program.TimeInSeconds.Should().Be(60);
        program.Power.Should().Be(5);
        program.Character.Should().Be('~');
        program.Instructions.Should().Be("Minhas instruções");
        program.IsPreset.Should().BeFalse();
        program.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateCustomHeating_WithoutInstructions_ShouldHaveNullInstructions()
    {
        // Arrange & Act
        var program = HeatingProgram.CreateCustomHeating(
            "Programa Simples", "Alimento", 30, 10, '!');

        // Assert
        program.Instructions.Should().BeNull();
    }

    [Theory]
    [InlineData("Pipoca", "Pipoca (de micro-ondas)", 180, 7, '*')]
    [InlineData("Leite", "Leite", 300, 5, '#')]
    [InlineData("Carnes de boi", "Carne em pedaço ou fatias", 840, 4, '@')]
    [InlineData("Frango", "Frango (qualquer corte)", 480, 7, '&')]
    [InlineData("Feijão", "Feijão congelado", 480, 9, '%')]
    public void PresetPrograms_ShouldHaveCorrectSettings(
        string name, string food, int timeInSeconds, int power, char character)
    {
        // Arrange & Act
        var program = HeatingProgram.CreatePresetHeating(
            name, food, timeInSeconds, power, character, "Test instructions");

        // Assert
        program.Name.Should().Be(name);
        program.Food.Should().Be(food);
        program.TimeInSeconds.Should().Be(timeInSeconds);
        program.Power.Should().Be(power);
        program.Character.Should().Be(character);
        program.IsPreset.Should().BeTrue();
    }
}
