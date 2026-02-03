using Microsoft.Extensions.DependencyInjection;
using Web.Microondas.Application.Exceptions;
using Web.Microondas.Application.Services.Implementations;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Enums;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Test.Application.Services;

public class MicrowaveServiceTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IHeatingProgramRepository> _programRepositoryMock;
    private readonly IMicrowaveService _service;

    public MicrowaveServiceTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _programRepositoryMock = new Mock<IHeatingProgramRepository>();

        var scopeMock = new Mock<IServiceScope>();
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        
        scopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
        scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        
        _serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(scopeFactoryMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IHeatingProgramRepository)))
            .Returns(_programRepositoryMock.Object);

        _service = new MicrowaveService(_serviceProviderMock.Object);
    }

    [Fact]
    public void GetState_InitialState_ShouldBeIdle()
    {
        // Act
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Idle.ToString());
        state.Power.Should().Be(10);
        state.RemainingSeconds.Should().Be(0);
    }

    [Fact]
    public void QuickStart_ShouldStartWith30Seconds()
    {
        // Act
        _service.QuickStart();
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Running.ToString());
        state.TimeInSeconds.Should().Be(30);
        state.Power.Should().Be(10);
        state.RemainingSeconds.Should().Be(30);
    }

    [Fact]
    public void StartManual_ShouldSetCorrectParameters()
    {
        // Act
        _service.StartManual(90, 7);
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Running.ToString());
        state.TimeInSeconds.Should().Be(90);
        state.Power.Should().Be(7);
        state.RemainingSeconds.Should().Be(90);
    }

    [Fact]
    public void Tick_ShouldUpdateState()
    {
        // Arrange
        _service.StartManual(10, 3);

        // Act
        _service.Tick();
        var state = _service.GetState();

        // Assert
        state.RemainingSeconds.Should().Be(9);
        state.ElapsedSeconds.Should().Be(1);
        state.Display.Should().NotBeEmpty();
    }

    [Fact]
    public void PauseOrCancel_WhenRunning_ShouldPause()
    {
        // Arrange
        _service.StartManual(60, 5);

        // Act
        _service.PauseOrCancel();
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Paused.ToString());
    }

    [Fact]
    public void PauseOrCancel_WhenPaused_ShouldCancel()
    {
        // Arrange
        _service.StartManual(60, 5);
        _service.PauseOrCancel();

        // Act
        _service.PauseOrCancel();
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Idle.ToString());
    }

    [Fact]
    public void Resume_WhenPaused_ShouldResume()
    {
        // Arrange
        _service.StartManual(60, 5);
        _service.PauseOrCancel();

        // Act
        _service.Resume();
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Running.ToString());
    }

    [Fact]
    public void Reset_ShouldResetToInitialState()
    {
        // Arrange
        _service.StartManual(60, 5);

        // Act
        _service.Reset();
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Idle.ToString());
        state.TimeInSeconds.Should().BeNull();
        state.RemainingSeconds.Should().Be(0);
    }

    [Fact]
    public void StartProgram_WithValidProgramId_ShouldStartProgram()
    {
        // Arrange
        var program = HeatingProgram.CreatePresetHeating(
            "Test", "Food", 120, 8, '*', "Instructions");
        
        _programRepositoryMock.Setup(x => x.GetByIdAsync(program.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(program);

        // Act
        _service.StartProgram(program.Id);
        var state = _service.GetState();

        // Assert
        state.State.Should().Be(HeatingState.Running.ToString());
        state.Power.Should().Be(8);
        state.Character.Should().Be('*');
        state.CurrentProgramId.Should().Be(program.Id);
    }

    [Fact]
    public void StartProgram_WithInvalidProgramId_ShouldThrowException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _programRepositoryMock.Setup(x => x.GetByIdAsync(invalidId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HeatingProgram?)null);

        // Act
        var act = () => _service.StartProgram(invalidId);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage($"Programa de aquecimento com ID {invalidId} não encontrado.");
    }
}
