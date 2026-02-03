using Web.Microondas.Application.Exceptions;

namespace Web.Microondas.Test.Application.Exceptions;

public class BusinessRuleExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "This is a business rule violation";

        // Act
        var exception = new BusinessRuleException(message);

        // Assert
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void BusinessRuleException_ShouldBeException()
    {
        // Arrange
        var exception = new BusinessRuleException("Test");

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }

    [Theory]
    [InlineData("Não é permitido adicionar tempo a programas pré-definidos.")]
    [InlineData("Programa de aquecimento não encontrado.")]
    [InlineData("Tempo deve estar entre 1 segundo e 2 minutos.")]
    public void Constructor_WithDifferentMessages_ShouldPreserveMessage(string message)
    {
        // Act
        var exception = new BusinessRuleException(message);

        // Assert
        exception.Message.Should().Be(message);
    }
}
