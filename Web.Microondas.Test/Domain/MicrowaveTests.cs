using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Enums;
using Web.Microondas.Domain.DomainException;

namespace Web.Microondas.Test.Domain;

public class MicrowaveTests
{
    [Fact]
    public void QuickStart_ShouldStartWith30SecondsAndPower10()
    {
        // Arrange
        var microwave = new Microwave();

        // Act
        microwave.QuickStart();

        // Assert
        microwave.TimeInSeconds.Should().Be(30);
        microwave.Power.Should().Be(10);
        microwave.RemainingSeconds.Should().Be(30);
        microwave.State.Should().Be(HeatingState.Running);
        microwave.Character.Should().Be('.');
    }

    [Fact]
    public void QuickStart_WhenRunning_ShouldAdd30Seconds()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.QuickStart();

        // Act
        microwave.QuickStart();

        // Assert
        microwave.RemainingSeconds.Should().Be(60);
    }

    [Fact]
    public void StartManual_ShouldSetTimeAndPower()
    {
        // Arrange
        var microwave = new Microwave();

        // Act
        microwave.StartManual(90, 5);

        // Assert
        microwave.TimeInSeconds.Should().Be(90);
        microwave.Power.Should().Be(5);
        microwave.RemainingSeconds.Should().Be(90);
        microwave.State.Should().Be(HeatingState.Running);
    }

    [Fact]
    public void StartManual_WhenRunning_ShouldAdd30Seconds()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(60, 8);

        // Act
        microwave.StartManual(30, 5);

        // Assert
        microwave.RemainingSeconds.Should().Be(90);
        microwave.Power.Should().Be(8); // Power should not change
    }

    [Fact]
    public void Tick_ShouldDecrementTimeAndUpdateDisplay()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(10, 3);

        // Act
        microwave.Tick();

        // Assert
        microwave.RemainingSeconds.Should().Be(9);
        microwave.ElapsedSeconds.Should().Be(1);
        microwave.Display.Should().HaveLength(3); // Power 3 = 3 characters
    }

    [Fact]
    public void Tick_WhenTimeReachesZero_ShouldComplete()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(1, 2);

        // Act
        microwave.Tick();

        // Assert
        microwave.State.Should().Be(HeatingState.Completed);
        microwave.RemainingSeconds.Should().Be(0);
    }

    [Fact]
    public void GetDisplayTime_ShouldFormatCorrectly()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(90, 5);

        // Act
        var displayTime = microwave.GetDisplayTime();

        // Assert
        displayTime.Should().Be("1:30");
    }

    [Fact]
    public void GetDisplayTime_ShouldShowZeroMinutes()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(45, 5);

        // Act
        var displayTime = microwave.GetDisplayTime();

        // Assert
        displayTime.Should().Be("0:45");
    }

    [Fact]
    public void GetFinalDisplay_WhenCompleted_ShouldIncludeCompletionMessage()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(1, 2);
        microwave.Tick();

        // Act
        var finalDisplay = microwave.GetFinalDisplay();

        // Assert
        finalDisplay.Should().Contain("Aquecimento concluído");
    }

    [Fact]
    public void PauseCancelButton_WhenRunning_ShouldPause()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(60, 5);

        // Act
        microwave.PauseCancelButton();

        // Assert
        microwave.State.Should().Be(HeatingState.Paused);
    }

    [Fact]
    public void PauseCancelButton_WhenPaused_ShouldReset()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(60, 5);
        microwave.PauseCancelButton();

        // Act
        microwave.PauseCancelButton();

        // Assert
        microwave.State.Should().Be(HeatingState.Idle);
        microwave.TimeInSeconds.Should().BeNull();
        microwave.RemainingSeconds.Should().Be(0);
    }

    [Fact]
    public void PauseCancelButton_WhenIdle_ShouldReset()
    {
        // Arrange
        var microwave = new Microwave();

        // Act
        microwave.PauseCancelButton();

        // Assert
        microwave.State.Should().Be(HeatingState.Idle);
    }

    [Fact]
    public void Resume_WhenPaused_ShouldResume()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(60, 5);
        microwave.PauseCancelButton();

        // Act
        microwave.Resume();

        // Assert
        microwave.State.Should().Be(HeatingState.Running);
    }

    [Fact]
    public void StartProgram_ShouldSetProgramProperties()
    {
        // Arrange
        var microwave = new Microwave();
        var program = HeatingProgram.CreatePresetHeating(
            "Test Program", "Test Food", 120, 7, '*', "Test instructions");

        // Act
        microwave.StartProgram(program);

        // Assert
        microwave.TimeInSeconds.Should().Be(120);
        microwave.Power.Should().Be(7);
        microwave.Character.Should().Be('*');
        microwave.CurrentProgramId.Should().Be(program.Id);
        microwave.State.Should().Be(HeatingState.Running);
    }

    [Fact]
    public void StartProgram_WhenRunningWithProgram_ShouldThrowException()
    {
        // Arrange
        var microwave = new Microwave();
        var program = HeatingProgram.CreatePresetHeating(
            "Test Program", "Test Food", 120, 7, '*', "Test instructions");
        microwave.StartProgram(program);

        // Act & Assert
        var act = () => microwave.StartProgram(program);
        act.Should().Throw<DomainException>()
            .WithMessage("Não é permitido adicionar tempo a programas pré-definidos.");
    }

    [Fact]
    public void AddTime_WhenRunningWithProgram_ShouldThrowException()
    {
        // Arrange
        var microwave = new Microwave();
        var program = HeatingProgram.CreatePresetHeating(
            "Test Program", "Test Food", 120, 7, '*', "Test instructions");
        microwave.StartProgram(program);

        // Act & Assert
        var act = () => microwave.QuickStart();
        act.Should().Throw<DomainException>()
            .WithMessage("Não é permitido adicionar tempo a programas pré-definidos.");
    }

    [Fact]
    public void AddTime_ShouldNotExceed120Seconds()
    {
        // Arrange
        var microwave = new Microwave();
        microwave.StartManual(100, 5);

        // Act
        microwave.QuickStart(); // Try to add 30 seconds

        // Assert
        microwave.RemainingSeconds.Should().Be(120); // Should cap at 120
    }
}
