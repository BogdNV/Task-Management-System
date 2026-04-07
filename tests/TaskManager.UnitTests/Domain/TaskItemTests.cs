using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.UnitTests.Domain;

public class TaskItemTests
{
    [Fact]
    public void Constructor_NewTask_SetsInitialProperties()
    {
        // Arrange
        var title = "Тестовая задача";
        var dueDate = DateTime.Now.AddDays(1);

        // Act
        var task = new TaskItem(title, "Описание", 1, dueDate);

        // Assert
        Assert.Equal(title, task.Title);
        Assert.Equal(dueDate, task.DueDate);
        // Проверка дефолтных значений для нового объекта
        Assert.Equal(default(int), task.Id);
        Assert.Equal(Status.New, task.Status);
        Assert.Equal(Priority.Medium, task.Priority);
    }

    [Fact]
    public void Constructor_ExistingTask_RestoresAllProperties()
    {
        // Arrange
        int id = 10;
        var status = Status.InProgress;
        var priority = Priority.High;

        // Act
        var task = new TaskItem(id, "Заголовок", "Описание", status, priority, 1, null);

        // Assert
        Assert.Equal(id, task.Id);
        Assert.Equal(status, task.Status);
        Assert.Equal(priority, task.Priority);
    }

    [Theory]
    [InlineData("", "Название не может быть пустым")]
    [InlineData(null, "Название не может быть пустым")]
    [InlineData("  ", "Название состоит из пробелов")]
    public void ValidateTitle_WithInvalidTitle_ThrowsArgumentException(string invalidTitle, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => new TaskItem(invalidTitle, "desc", 1, null));

        Assert.Contains(expectedMessage, ex.Message);
    }

    [Fact]
    public void ValidateDueDate_DateOfThePast_ThrowsArgumentException()
    {
        var pastDate = DateTime.UtcNow.AddSeconds(-3);
        var ex = Assert.Throws<ArgumentException>(() => new TaskItem("Title", "desc", 1, pastDate));

        Assert.Contains("Дедлайн не может быть в прошлом", ex.Message);
    }

    [Theory]
    [InlineData("", "Описание задачи не может быть пустым")]
    [InlineData(null, "Описание задачи не может быть пустым")]
    [InlineData("  ", "Описание задачи не может быть пустым")]
    public void ValidateDescription_WithInvalidDescription_ThrowsArgumentException(string invalidDesc, string expectedMessage)
    {
        // Given

        // When
        var ex = Assert.Throws<ArgumentException>(() => new TaskItem("title", invalidDesc, 1, null));

        // Then
        Assert.Contains(expectedMessage, ex.Message);
    }

    [Fact]
    public void UpdateDueDate_WithValidDate_SchouldUpdateProperty()
    {
        // Given
        var newDate = DateTime.UtcNow.AddDays(3);
        var task = new TaskItem("title", "desc", 1, null);
        // When
        task.UpdateDueDate(newDate);
        // Then
        Assert.Equal(newDate, task.DueDate);
    }

    [Fact]
    public void UpdateDueDate_WithInvalidDate_ThrowsArgumentException()
    {
        // Given
        var invalidDate = DateTime.UtcNow.AddSeconds(-3);
        var task = new TaskItem("title", "desc", 1, null);
        // When
        var ex = Assert.Throws<ArgumentException>(() => task.UpdateDueDate(invalidDate));
        // Then
        Assert.Contains("Дедлайн не может быть в прошлом", ex.Message);
    }

    [Theory]
    [InlineData(Status.New)]
    [InlineData(Status.InProgress)]
    [InlineData(Status.Done)]
    public void ChangeStatus_UpdateStatus_Success(Status status)
    {
        // Given
        var task = new TaskItem("title", "desc", 1, null);
        // When
        task.ChangeStatus(status);
        // Then
        Assert.Equal(status, task.Status);
    }

    [Theory]
    [InlineData(Status.InProgress)]
    [InlineData(Status.New)]
    public void ChangeStatus_UpdateStatusDone_ThrowInvalidOperationException(Status status)
    {
        // Given
        var task = new TaskItem("title", "desc", 1, null);
        task.ChangeStatus(Status.Done);
        // When
        var ex = Assert.Throws<InvalidOperationException>(() => task.ChangeStatus(status));
        // Then
        Assert.Contains("Нельзя изменить статус выполненой задачи", ex.Message);
    }

    [Theory]
    [InlineData(Priority.High)]
    [InlineData(Priority.Low)]
    [InlineData(Priority.Medium)]
    public void UpdatePriority_UpdatePriority_Success(Priority priority)
    {
        // Given
        var task = new TaskItem("title", "desc", 1, null);
        // When
        task.UpdatePriority(priority);
        // Then
        Assert.Equal(priority, task.Priority);
    }

    [Fact]
    public void AssignToProject_AddTaskToProject_Success()
    {
        // Given
        var task = new TaskItem("title", "desc", 1, null);
        var project = new Project(1, "Test project", "Tests", 1, DateTime.UtcNow);
        // When
        task.AssignToProject(project);
        // Then
        Assert.Equal(project.Id, task.ProjectId);
        Assert.Single(project.Tasks);
    }

    [Fact]
    public void AssignToProject_InvalidIdToProject_ThrowArgumentException()
    {
        // Given
        var task = new TaskItem("title", "desc", 1, null);
        var project = new Project("Test project", "Tests", 1);
        // When
        var ex = Assert.Throws<ArgumentException>(() => task.AssignToProject(project));
        // Then
        Assert.Contains("ID проекта должен быть положительным", ex.Message);
    }
}
