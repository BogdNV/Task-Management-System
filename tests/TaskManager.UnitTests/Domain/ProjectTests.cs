using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.Domain;

public class ProjectTests
{
    [Fact]
    public void Ctor_WithValidData_ShouldInitializeCorrectly()
    {
        var ctorThreeParameters = new Project("Valid Project name", "Some description", 1);
        var utcNow = DateTime.UtcNow;
        var ctorFiveParameters = new Project(42, "Valid name", "desc", 3, utcNow);

        Assert.Equal("Valid Project name", ctorThreeParameters.Name);
        Assert.Equal("Some description", ctorThreeParameters.Description);
        Assert.Equal(1, ctorThreeParameters.OwnerId);
        Assert.Equal(0, ctorThreeParameters.Id);
        Assert.Empty(ctorThreeParameters.Tasks);
        Assert.InRange((DateTime.UtcNow - ctorThreeParameters.CreatedAt).TotalSeconds, 0, 2);

        Assert.Equal("Valid name", ctorFiveParameters.Name);
        Assert.Equal("desc", ctorFiveParameters.Description);
        Assert.Equal(3, ctorFiveParameters.OwnerId);
        Assert.Equal(42, ctorFiveParameters.Id);
        Assert.Equal(utcNow, ctorFiveParameters.CreatedAt);
        Assert.Empty(ctorFiveParameters.Tasks);
    }

    [Theory]
    [InlineData("", "Название не должно быть пустым")]
    [InlineData(null, "Название не должно быть пустым")]
    [InlineData("abc", "Название должно быть длиной минимум 5 символов")]
    public void Ctor_WithNameTooShortNullOrEmpty_ThrowsArgumentException(string invalidName, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Project(invalidName, "desc", 1));

        Assert.Contains(expectedMessage, ex.Message);
    }

    [Theory]
    [InlineData("", "Описание не должно быть пустым")]
    [InlineData(null, "Описание не должно быть пустым")]
    [InlineData("   ", "Описание состоит из пробелов")]
    public void Ctor_WithDescriptionWhiteSpaceOrNullOrEmpty_ThrowsArgumentException(string invalidDescription, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Project("Tests", invalidDescription, 1));

        Assert.Contains(expectedMessage, ex.Message);
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateProperty()
    {
        var proj = new Project("Valid Project name", "Some description", 1);

        proj.UpdateName("NewName");

        Assert.Equal("NewName", proj.Name);
    }

    [Theory]
    [InlineData(null, "Название не должно быть пустым")]
    [InlineData("", "Название не должно быть пустым")]
    [InlineData("abc", "Название должно быть длиной минимум 5 символов")]
    public void UpdateName_WithInvalidName_ShouldThrowArgumentException(string? invalidName, string expectedMessage)
    {
        var proj = new Project("Valid Project name", "Some description", 1);

        var ex = Assert.Throws<ArgumentException>(() => proj.UpdateName(invalidName));

        Assert.Contains(expectedMessage, ex.Message);
    }

    [Fact]
    public void UpdateDescription_WithValidDescription_ShouldUpdateProperty()
    {
        var proj = new Project("Valid Project name", "Some description", 1);

        proj.UpdateDescription("desc");

        Assert.Equal("desc", proj.Description);
    }

    [Theory]
    [InlineData("", "Описание не должно быть пустым")]
    [InlineData(null, "Описание не должно быть пустым")]
    [InlineData("  ", "Описание состоит из пробелов")]
    public void UpdateDescription_WithNullEmptyOrWhitespace_ShouldThrowArgumentException(string invalidDescription, string expectedMessage)
    {
        var proj = new Project("Valid Project name", "Some description", 1);

        var ex = Assert.Throws<ArgumentException>(() => proj.UpdateDescription(invalidDescription));

        Assert.Contains(expectedMessage, ex.Message);

    }

    [Fact]
    public void UpdateDescription_ExceedsMaxLength_ShouldThrowArgumentException()
    {
        var proj = new Project("Valid Project name", "Some description", 1);
        var newDescription = new string('a', Project.MAX_DESCRIPTION_LENGTH + 1);

        var ex = Assert.Throws<ArgumentException>(() => proj.UpdateDescription(newDescription));

        Assert.Contains($"Превышен порог в {Project.MAX_DESCRIPTION_LENGTH} символов", ex.Message);
    }

    [Fact]
    public void AddTask_WithNullTask_ShouldThrowArgumentNullException()
    {
        var proj = new Project("Valid Project name", "Some description", 1);

        var ex = Assert.Throws<ArgumentNullException>(() => proj.AddTask(null));

        Assert.Contains("Задача не может быть null", ex.Message);
    }
}
