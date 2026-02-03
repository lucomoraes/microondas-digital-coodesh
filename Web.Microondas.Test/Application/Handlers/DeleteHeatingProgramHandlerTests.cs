using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;

namespace Web.Microondas.Test.Application.Handlers;

public class DeleteHeatingProgramHandlerTests
{
    private readonly Mock<IHeatingProgramRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteHeatingProgramHandler _handler;

    public DeleteHeatingProgramHandlerTests()
    {
        _repositoryMock = new Mock<IHeatingProgramRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteHeatingProgramHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldDeleteProgram()
    {
        // Arrange
        var programId = Guid.NewGuid();
        var request = new DeleteHeatingProgramRequest { Id = programId };

        _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteAsync(programId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ShouldRollback()
    {
        // Arrange
        var request = new DeleteHeatingProgramRequest { Id = Guid.NewGuid() };

        _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        _unitOfWorkMock.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
