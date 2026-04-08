using FluentAssertions;
using Moq;
using TaskManager.Application.Projects.Commands.CreateProject;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.UnitTests.Application.Projects;

public class CreateProjectHandlerTests
{
    private readonly Mock<IProjectRepository> _repoMock;
    private readonly CreateProjectHandler _handler;

    public CreateProjectHandlerTests()
    {
        _repoMock = new Mock<IProjectRepository>();
        _handler = new CreateProjectHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProjectAndReturnDto()
    {
        // Given
        var command = new CreateProjectCommand("Test Project", "Desc", 1);
        var expectedProject = new Project(1, "Test Project", "Desc", 1, DateTime.UtcNow);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(expectedProject);
        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test Project");

        _repoMock.Verify(r => r.AddAsync(It.Is<Project>(
            p => p.Name == command.Name && p.OwnerId == command.OwnerId
        )), Times.Once);
    }

    [Theory]
    [InlineData("", "Название не должно быть пустым*")]
    [InlineData(null, "Название не должно быть пустым*")]
    [InlineData("test", "Название должно быть длиной минимум 5 символов*")]
    public async Task Handle_InvalidName_ShouldThrowArgumenException(string name, string expectedMessage)
    {
        // Given
        var command = new CreateProjectCommand(name, "desc", 1);
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("", "Описание не должно быть пустым*")]
    [InlineData(null, "Описание не должно быть пустым*")]
    [InlineData("  ", "Описание состоит из пробелов*")]
    public async Task Handle_InvalidDescription_ShouldThrowArgumentException(string desc, string expectedMessage)
    {
        // Given
        var command = new CreateProjectCommand("Test name", desc, 1);
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage(expectedMessage);

    }

    [Fact]
    public async Task Handle_DescriptionTooLong_ShouldThrowArgumentException()
    {
        // Given
        var command = new CreateProjectCommand("Test name", new string('a', Project.MAX_DESCRIPTION_LENGTH + 1), 1);
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage($"Превышен порог в {Project.MAX_DESCRIPTION_LENGTH} символов*");
    }

    [Fact]
    public async Task Handle_OwnerIdNegative_ShouldThrowArgumentException()
    {
        // Given
        var command = new CreateProjectCommand("Test name", "desc", -1);
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Неверный ID владельца*");
    }
}
