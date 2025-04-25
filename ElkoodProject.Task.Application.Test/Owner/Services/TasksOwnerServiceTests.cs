namespace ElkoodProject.Task.Application.Test.Owner.Services;

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Domain.Tasks.Repositories;
using ElkoodProject.Task.Application.Owner.Mappers;
using ElkoodProject.Task.Application.Owner.Services;
using FluentAssertions;
using Moq;
using NSubstitute;

public class TasksOwnerServiceTests
{
    private readonly Fixture _fixture;

    public TasksOwnerServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        // auto mapper configuration
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TaskDtoOwnerMappingProfile());
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
        var sut = _fixture.Create<TasksOwnerService>();

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
        var result = mapper.Map<TaskListItemsDto>(tasksList);

        var moq = new Mock<ITasksRepository>();

        moq.Setup(repo => repo.GetAllAsync(It.IsAny<TaskFilter>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(tasksList);

        var sut = new TasksOwnerService(moq.Object, mapper);

        // Act
        var returnedValue = await sut.GetAllAsync(taskFilterDto, 1, 10);

        // Assert
        returnedValue.Items.Should().BeEquivalentTo(result.Items);
    }

    //AddAsync
    [Fact]
    public async System.Threading.Tasks.Task AddAsync_Should_ThrowArgumentNullException_When_AddTaskDtoIsNull()
    {
        // Arrange
        var sut = _fixture.Create<TasksOwnerService>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.AddAsync(null!));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_Should_ReturnExpectedResult()
    {
        // Arrange
        var mapper = _fixture.Create<IMapper>();
        var addTaskDto = _fixture.Create<AddTaskDto>();
        var task = _fixture.Create<Task>();
        var result = mapper.Map<TaskDto>(task);

        var moq = new Mock<ITasksRepository>();

        moq.Setup(repo => repo.AddAsync(It.IsAny<Task>()))
            .ReturnsAsync(task);

        var sut = new TasksOwnerService(moq.Object, mapper);

        // Act
        var returnedValue = await sut.AddAsync(addTaskDto);

        // Assert
        returnedValue.Should().BeEquivalentTo(result);
    }

    //UpdateAsync
    [Fact]
    public async System.Threading.Tasks.Task UpdateAsync_Should_ThrowArgumentNullException_When_AddTaskDtoIsNull()
    {
        // Arrange
        var sut = _fixture.Create<TasksOwnerService>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.UpdateAsync(Guid.NewGuid(), null!));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateAsync_Should_ReturnExpectedResult()
    {
        // Arrange
        var mapper = _fixture.Create<IMapper>();
        var id = _fixture.Create<Guid>();
        var updateTaskDto = _fixture.Create<UpdateTaskDto>();
        var task = _fixture.Create<Task>();
        var result = mapper.Map<TaskDto>(task);

        var moq = new Mock<ITasksRepository>();

        moq.Setup(repo => repo.UpdateAsync(id, It.IsAny<Task>()))
            .ReturnsAsync(task);

        var sut = new TasksOwnerService(moq.Object, mapper);

        // Act
        var returnedValue = await sut.UpdateAsync(id, updateTaskDto);

        // Assert
        returnedValue.Should().BeEquivalentTo(result);
    }
}
