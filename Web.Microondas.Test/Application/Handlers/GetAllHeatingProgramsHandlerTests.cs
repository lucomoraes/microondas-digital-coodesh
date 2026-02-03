using Web.Microondas.Application.UseCases.HeatingProgram.Handlers;
using Web.Microondas.Application.UseCases.HeatingProgram.Queries;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Test.Application.Handlers;

public class GetAllHeatingProgramsHandlerTests
{
    private readonly Mock<IHeatingProgramRepository> _repositoryMock;
    private readonly GetAllHeatingProgramsHandler _handler;

    public GetAllHeatingProgramsHandlerTests()
    {
        _repositoryMock = new Mock<IHeatingProgramRepository>();
        _handler = new GetAllHeatingProgramsHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllPrograms()
    {
        // Arrange
        var programs = new List<HeatingProgram>
        {
            HeatingProgram.CreatePresetHeating("Pipoca", "Pipoca", 180, 7, '*', "Test"),
            HeatingProgram.CreatePresetHeating("Leite", "Leite", 300, 5, '#', "Test"),
            HeatingProgram.CreateCustomHeating("Custom", "Food", 60, 5, '~')
        };

        _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(programs);

        // Act
        var result = await _handler.Handle(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(p => p.Name == "Pipoca");
        result.Should().Contain(p => p.Name == "Leite");
        result.Should().Contain(p => p.Name == "Custom");
    }

    [Fact]
    public async Task Handle_WhenNoPrograms_ShouldReturnEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<HeatingProgram>());

        // Act
        var result = await _handler.Handle(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldDifferentiatePresetAndCustomPrograms()
    {
        // Arrange
        var programs = new List<HeatingProgram>
        {
            HeatingProgram.CreatePresetHeating("Preset1", "Food1", 60, 5, '*', "Test"),
            HeatingProgram.CreateCustomHeating("Custom1", "Food2", 60, 5, '~'),
        };

        _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(programs);

        // Act
        var result = await _handler.Handle(CancellationToken.None);

        // Assert
        var preset = result.First(p => p.Name == "Preset1");
        var custom = result.First(p => p.Name == "Custom1");

        preset.IsPreset.Should().BeTrue();
        custom.IsPreset.Should().BeFalse();
    }
}
