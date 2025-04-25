namespace ElkoodProject.Task.Application.Test.Owner.Validators;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Task.Application.Owner.Validators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

public class UpdateTaskDtoValidatorTests
{
    private const string InvalidDescription = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789" +
                                              "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789" +
                                              "012345678901234567890123456789012345678901234567890123456";
    private readonly Fixture _fixture;

    public UpdateTaskDtoValidatorTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Theory]
    [InlineData("me")]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234")]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Invalid_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.Name, name)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Theory]
    [InlineData("0123456789012345678")]
    [InlineData(InvalidDescription)]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Invalid_When_Description_Is_Invalid(string description)
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.Description, description)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Invalid_When_DiedLineInHours_Is_Invalid(int diedLineInHours)
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.DiedLineInHours, diedLineInHours)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DiedLineInHours)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Invalid_When_Status_Is_Invalid()
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.Status, (TaskStatus)3)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Invalid_When_Category_Is_Invalid()
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.Category, (TaskCategory)4)
                                 .With(a => a.Status, (TaskStatus)1)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Invalid_When_Priority_Is_Invalid()
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.Priority, -1)
                                 .With(a => a.Status, (TaskStatus)1)
                                 .With(a => a.Category, (TaskCategory)1)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Priority)
              .WithSeverity(Severity.Error);

        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskDtoValidator_Should_Be_Valid_When_All_Properties_Is_Valid()
    {
        // Arrange
        var updateTaskDto = _fixture.Build<UpdateTaskDto>()
                                 .With(a => a.Priority, 2)
                                 .With(a => a.Name, "fajr")
                                 .With(a => a.Description, "fajrfajrfajrfajrfajrfajr")
                                 .With(a => a.Status, (TaskStatus)1)
                                 .With(a => a.Category, (TaskCategory)1)
                                 .With(a => a.DiedLineInHours, 5)
                                 .Create();

        var validator = _fixture.Create<UpdateTaskDtoValidator>();

        // Act
        var result = await validator.TestValidateAsync(updateTaskDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();

        result.Errors.Count.Should().Be(0);
    }
}
