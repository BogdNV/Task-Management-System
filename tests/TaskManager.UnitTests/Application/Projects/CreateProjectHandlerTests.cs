using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Application.Projects.Commands;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.UnitTests.Application.Projects;

public class CreateProjectHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly ApplicationDbContext _context;
    private readonly CreateProjectHandler _handler;

    public CreateProjectHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CreateProjectTestDb")
            .Options;
        _context = new ApplicationDbContext(options);
        _handler = new CreateProjectHandler(_context, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProjectAndReturnDto()
    {
        // Given
        var command = new CreateProjectCommand
        {
            Title = "Test Project",
            Description = "Test Desc",
            UserId = Guid.NewGuid()
        };

        _mapperMock.Setup(m => m.Map<ProjectEntity>(It.IsAny<CreateProjectCommand>()))
            .Returns((CreateProjectCommand c) => new ProjectEntity
            {
                Title = c.Title,
                Description = c.Description,
                UserId = c.UserId
            });

        _mapperMock.Setup(m => m.Map<CreateProjectCommandResult>(It.IsAny<ProjectEntity>()))
            .Returns((ProjectEntity e) => new CreateProjectCommandResult
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                UserId = e.UserId,
                CreatedAt = e.CreatedAt
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be("Test Project");
        result.Description.Should().Be("Test Desc");
        result.UserId.Should().Be(command.UserId);
    }
}
