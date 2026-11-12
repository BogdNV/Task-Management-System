using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Projects.Commands;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.UnitTests.Application.Projects;

public class PutProjectHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly ApplicationDbContext _context;
    private readonly PutProjectHandler _handler;

    public PutProjectHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PutProjectTestDb")
            .Options;
        _context = new ApplicationDbContext(options);
        _handler = new PutProjectHandler(_context, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateProject()
    {
        // Given
        var userId = Guid.NewGuid();
        var existingProject = new ProjectEntity
        {
            Id = 1,
            Title = "Old Name",
            Description = "Old Desc",
            UserId = userId
        };
        _context.Projects.Add(existingProject);
        await _context.SaveChangesAsync();

        var command = new PutProjectCommand
        {
            Id = existingProject.Id,
            UserId = userId,
            Title = "New Name",
            Description = "New Desc"
        };

        _mapperMock.Setup(m => m.Map<PutProjectCommandResult>(It.IsAny<ProjectEntity>()))
            .Returns((ProjectEntity e) => new PutProjectCommandResult
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                UserId = e.UserId
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Title.Should().Be("New Name");
        result.Description.Should().Be("New Desc");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Handle_NonExistentProject_ShouldThrowNotFoundException()
    {
        // Given
        var command = new PutProjectCommand
        {
            Id = 999,
            UserId = Guid.NewGuid(),
            Title = "New Name",
            Description = "New Desc"
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
