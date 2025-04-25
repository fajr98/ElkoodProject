namespace ElkoodProject.Task.Application.Test.Owner.Validators;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Task.Application.Owner.Validators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

public class TaskFilterDtoValidatorTests
{
    private const string InvalidDescription = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789" +
                                              "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789" +
                                              "012345678901234567890123456789012345678901234567890123456";
    private readonly Fixture _fixture;

    public TaskFilterDtoValidatorTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Theory]
    [InlineData("me")]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234")]
    public async System.Threading.Tasks.Task TaskFilterDtoValidator_Should_Be_Invalid_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var TaskFilterDto = _fixture.Build<TaskFilterDto>()
                                 .With(a => a.Name, name)
                                 .Create();

        var validator = _fixture.Create<TaskFilterDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(TaskFilterDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    public async System.Threading.Tasks.Task TaskFilterDtoValidator_Should_Be_Invalid_When_DiedLineInHours_Is_Invalid(int diedLineInHours)
    {
        // Arrange
        var TaskFilterDto = _fixture.Build<TaskFilterDto>()
                                 .With(a => a.DiedLineInHours, diedLineInHours)
                                 .Create();

        var validator = _fixture.Create<TaskFilterDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(TaskFilterDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DiedLineInHours)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskFilterDtoValidator_Should_Be_Invalid_When_Status_Is_Invalid()
    {
        // Arrange
        var TaskFilterDto = _fixture.Build<TaskFilterDto>()
                                 .With(a => a.Status, (TaskStatus)3)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<TaskFilterDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(TaskFilterDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskFilterDtoValidator_Should_Be_Invalid_When_Category_Is_Invalid()
    {
        // Arrange
        var TaskFilterDto = _fixture.Build<TaskFilterDto>()
                                 .With(a => a.Category, (TaskCategory)4)
                                 .With(a => a.Status, (TaskStatus)1)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<TaskFilterDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(TaskFilterDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskFilterDtoValidator_Should_Be_Invalid_When_Priority_Is_Invalid()
    {
        // Arrange
        var TaskFilterDto = _fixture.Build<TaskFilterDto>()
                                 .With(a => a.Priority, -1)
                                 .With(a => a.Status, (TaskStatus)1)
                                 .With(a => a.Category, (TaskCategory)1)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<TaskFilterDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(TaskFilterDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Priority)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskFilterDtoValidator_Should_Be_Valid_When_All_Properties_Is_Valid()
    {
        // Arrange
        var TaskFilterDto = _fixture.Build<TaskFilterDto>()
                                 .With(a => a.Priority, 2)
                                 .With(a => a.Name, "fajr")
                                 .With(a => a.Status, (TaskStatus)1)
                                 .With(a => a.Category, (TaskCategory)1)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<TaskFilterDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(TaskFilterDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();

        result.Errors.Count.Should().Be(0);
    }
}
