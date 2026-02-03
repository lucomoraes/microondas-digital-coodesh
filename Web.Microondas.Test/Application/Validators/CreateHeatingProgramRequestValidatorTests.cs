using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.Validators.HeatingProgram;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Test.Application.Validators;

public class CreateHeatingProgramRequestValidatorTests
{
    private readonly Mock<IHeatingProgramRepository> _repositoryMock;
    private readonly CreateHeatingProgramRequestValidator _validator;

    public CreateHeatingProgramRequestValidatorTests()
    {
        _repositoryMock = new Mock<IHeatingProgramRepository>();
        _validator = new CreateHeatingProgramRequestValidator(_repositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidRequest_ShouldPass()
    {
        // Arrange
        _repositoryMock.Setup(x => x.CheckIfExistsAsync(It.IsAny<char>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa Teste",
            Food = "Comida Teste",
            TimeInSeconds = 60,
            Power = 5,
            Character = '~',
            Instructions = "Instruções teste"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Comida", 60, 5, '~')]
    [InlineData(null, "Comida", 60, 5, '~')]
    public async Task Validate_WithEmptyName_ShouldFail(string name, string food, int time, int power, char character)
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = name,
            Food = food,
            TimeInSeconds = time,
            Power = power,
            Character = character
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Nome do programa é obrigatório"));
    }

    [Theory]
    [InlineData("Programa", "", 60, 5, '~')]
    [InlineData("Programa", null, 60, 5, '~')]
    public async Task Validate_WithEmptyFood_ShouldFail(string name, string food, int time, int power, char character)
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = name,
            Food = food,
            TimeInSeconds = time,
            Power = power,
            Character = character
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Alimento é obrigatório"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(121)]
    [InlineData(200)]
    public async Task Validate_WithInvalidTime_ShouldFail(int timeInSeconds)
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa",
            Food = "Comida",
            TimeInSeconds = timeInSeconds,
            Power = 5,
            Character = '~'
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Tempo deve estar entre 1 segundo e 2 minutos"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(15)]
    public async Task Validate_WithInvalidPower_ShouldFail(int power)
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa",
            Food = "Comida",
            TimeInSeconds = 60,
            Power = power,
            Character = '~'
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Potência deve estar entre 1 e 10"));
    }

    [Fact]
    public async Task Validate_WithReservedCharacter_ShouldFail()
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa",
            Food = "Comida",
            TimeInSeconds = 60,
            Power = 5,
            Character = '.'
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Caractere '.' é reservado"));
    }

    [Fact]
    public async Task Validate_WithExistingCharacter_ShouldFail()
    {
        // Arrange
        _repositoryMock.Setup(x => x.CheckIfExistsAsync('*', It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa",
            Food = "Comida",
            TimeInSeconds = 60,
            Power = 5,
            Character = '*'
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Caractere já está em uso"));
    }
}
