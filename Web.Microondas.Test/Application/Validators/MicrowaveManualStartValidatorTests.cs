using Web.Microondas.Application.Validators.Microwave;

namespace Web.Microondas.Test.Application.Validators;

public class MicrowaveManualStartValidatorTests
{
    private readonly MicrowaveManualStartValidator _validator;

    public MicrowaveManualStartValidatorTests()
    {
        _validator = new MicrowaveManualStartValidator();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(30, 5)]
    [InlineData(60, 10)]
    [InlineData(120, 1)]
    public void Validate_WithValidValues_ShouldPass(int seconds, int power)
    {
        // Arrange & Act
        var result = _validator.Validate((seconds, power));

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(-1, 5)]
    [InlineData(121, 5)]
    [InlineData(200, 5)]
    public void Validate_WithInvalidSeconds_ShouldFail(int seconds, int power)
    {
        // Arrange & Act
        var result = _validator.Validate((seconds, power));

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Tempo deve estar entre 1 segundo e 2 minutos"));
    }

    [Theory]
    [InlineData(60, 0)]
    [InlineData(60, -1)]
    [InlineData(60, 11)]
    [InlineData(60, 15)]
    public void Validate_WithInvalidPower_ShouldFail(int seconds, int power)
    {
        // Arrange & Act
        var result = _validator.Validate((seconds, power));

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Potência deve estar entre 1 e 10"));
    }

    [Fact]
    public void Validate_WithBothInvalid_ShouldFailWithMultipleErrors()
    {
        // Arrange & Act
        var result = _validator.Validate((0, 0));

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }
}
