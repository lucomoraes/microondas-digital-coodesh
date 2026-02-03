using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;

namespace Web.Microondas.Test.Application.Handlers;

public class CreateHeatingProgramHandlerTests
{
    private readonly Mock<IHeatingProgramRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateHeatingProgramHandler _handler;

    public CreateHeatingProgramHandlerTests()
    {
        _repositoryMock = new Mock<IHeatingProgramRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateHeatingProgramHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateProgram()
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa Teste",
            Food = "Comida Teste",
            TimeInSeconds = 60,
            Power = 5,
            Character = '~',
            Instructions = "Instruções teste"
        };

        _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Programa Teste");
        result.Food.Should().Be("Comida Teste");
        result.TimeInSeconds.Should().Be(60);
        result.Power.Should().Be(5);
        result.Character.Should().Be('~');
        result.IsPreset.Should().BeFalse();

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<HeatingProgram>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ShouldRollback()
    {
        // Arrange
        var request = new CreateHeatingProgramRequest
        {
            Name = "Programa Teste",
            Food = "Comida Teste",
            TimeInSeconds = 60,
            Power = 5,
            Character = '~'
        };

        _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<HeatingProgram>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        _unitOfWorkMock.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
