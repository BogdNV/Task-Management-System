using FluentAssertions;
using Moq;
using TaskManager.Application.Projects.Commands.UpdateProject;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.UnitTests.Application.Projects;

public class UpdateProjectHandlerTests
{
    private readonly Mock<IProjectRepository> _repoMock;
    private readonly UpdateProjectHandler _handler;

    public UpdateProjectHandlerTests()
    {
        _repoMock = new Mock<IProjectRepository>();
        _handler = new UpdateProjectHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateProject()
    {
        // Given
        var command = new UpdateProjectCommand(2, "New Name", "New desc");
        var createdAt = DateTime.UtcNow;
        var project = new Project(2, "Test name", "desc", 1, createdAt);

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(project);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Project>()));
        // When
        await _handler.Handle(command, CancellationToken.None);
        // Then
        Assert.Equal("New Name", project.Name);
        Assert.Equal(2, project.Id);
        Assert.Equal("New desc", project.Description);
        Assert.Equal(createdAt, project.CreatedAt);

        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentProjectId_NoAction()
    {
        // Given
        var project = new Project("Test Name", "Test Desc", 1);
        var command = new UpdateProjectCommand(1, "New Name", "New Desc");

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()));
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Project>()));
        // When
        await _handler.Handle(command, CancellationToken.None);
        // Then
        Assert.Equal("Test Name", project.Name);
        Assert.Equal("Test Desc", project.Description);

        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public void Handle_InvalidName_ShouldThrowArgumentException()
    {
        // Given
        var command = new UpdateProjectCommand(1, "Test", "Test Desc");
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Then
        act.Should().ThrowAsync<ArgumentException>().WithMessage(
            $"Название должно быть длиной минимум {Project.MIN_NAME_LENGTH} символов*");

        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public void Handle_InvalidDescription_ShouldArgumentException()
    {
        // Given
        var command = new UpdateProjectCommand(1, "Test", "    ");
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Then
        act.Should().ThrowAsync<ArgumentException>().WithMessage(
            $"Описание состоит из пробелов*"
        );

        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Never);
    }
}
