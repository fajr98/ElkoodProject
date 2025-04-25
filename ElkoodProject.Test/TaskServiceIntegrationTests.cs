namespace ElkoodProject.Test;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Owner.Contracts;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Domain.Tasks.Repositories;
using ElkoodProject.Task.Application.Owner.Mappers;
using ElkoodProject.Task.Application.Owner.Services;
using ElkoodProject.Task.DataAccess;
using ElkoodProject.Tasks.DataAccess.Mappers;
using ElkoodProject.Tasks.DataAccess.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class TaskServiceIntegrationTests
{
    private readonly ITasksOwnerService _taskService;
    private readonly ITasksRepository _taskRepository;
    private readonly TasksDbContext _dbContext;
    private readonly Fixture _fixture;

    public TaskServiceIntegrationTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        // auto mapper configuration
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TaskDtoOwnerMappingProfile());
            cfg.AddProfile(new TaskEntityMappingProfile());
        });

        var mapper = mapperConfiguration.CreateMapper();
        _fixture.Inject(mapper);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // Setup InMemoryDatabase for DbContext
        var options = new DbContextOptionsBuilder<TasksDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _dbContext = new TasksDbContext(options);
        _taskRepository = new TasksRepository(_dbContext, mapper);
        _taskService = new TasksOwnerService(_taskRepository, mapper);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddTask_Should_Persist_Task_In_Database()
    {
        // Arrange
        var mapper = _fixture.Create<IMapper>();
        var addTaskDto = new AddTaskDto
        {
            Name = "Test Task",
            Description = "Task Description",
            DiedLineInHours = 5,
            Category = TaskCategory.A,
            Status = TaskStatus.Incomplete,
            Priority = 2,
        };
        var taskToAdd = mapper.Map<Task>(addTaskDto);

        // Act
        var result = await _taskService.AddAsync(addTaskDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(taskToAdd.Name);
        result.Description.Should().Be(taskToAdd.Description);
        result.Priority.Should().Be(taskToAdd.Priority);

        var savedTask = await _taskRepository.GetByIdAsync(result.Id);

        // Assert: Ensure that the task is saved in the database
        savedTask.Should().NotBeNull();
        savedTask.Name.Should().Be(addTaskDto.Name);
        savedTask.Description.Should().Be(addTaskDto.Description);
        savedTask.Priority.Should().Be(addTaskDto.Priority);
    }
}


