namespace ElkoodProject.Task.Application.Test.Guest.Services;

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Guest.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Domain.Tasks.Repositories;
using ElkoodProject.Task.Application.Guest.Mappers;
using ElkoodProject.Task.Application.Guest.Services;
using FluentAssertions;
using Moq;

public class TasksGuestServiceTests
{
    private readonly Fixture _fixture;

    public TasksGuestServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        // auto mapper configuration
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TaskDtoGuestMappingProfile());
        });

        var mapper = mapperConfiguration.CreateMapper();
        _fixture.Inject(mapper);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    // GetAllAsync
    [Fact]
    public async System.Threading.Tasks.Task GetAllAsync_Should_ThrowArgumentNullException_When_TaskFilterDtoIsNull()
    {
        // Arrange
        var sut = _fixture.Create<TasksGuestService>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.GetAllAsync(null!, 1, 10));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllAsync_Should_ReturnExpectedResult()
    {
        // Arrange
        var mapper = _fixture.Create<IMapper>();
        var taskFilterDto = _fixture.Create<TaskFilterDto>();

        var taskFilter = mapper.Map<TaskFilter>(taskFilterDto);
        var tasksList = _fixture.Build<Task>().CreateMany().ToList();
        var result = mapper.Map<TaskListItemsGuestDto>(tasksList);

        var moq = new Mock<ITasksRepository>();

        moq.Setup(repo => repo.GetAllAsync(It.IsAny<TaskFilter>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(tasksList);

        var sut = new TasksGuestService(moq.Object, mapper);

        // Act
        var returnedValue = await sut.GetAllAsync(taskFilterDto, 1, 10);

        // Assert
        returnedValue.Items.Should().BeEquivalentTo(result.Items);
    }
}
