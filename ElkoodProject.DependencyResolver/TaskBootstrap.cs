namespace ElkoodProject.DependencyResolver;

using ElkoodProject.Application.Contracts.Task.Guest.Contracts;
using ElkoodProject.Application.Contracts.Task.Owner.Contracts;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Repositories;
using ElkoodProject.Task.Application.Guest.Mappers;
using ElkoodProject.Task.Application.Guest.Services;
using ElkoodProject.Task.Application.Owner.Mappers;
using ElkoodProject.Task.Application.Owner.Services;
using ElkoodProject.Task.Application.Owner.Validators;
using ElkoodProject.Tasks.DataAccess.Mappers;
using ElkoodProject.Tasks.DataAccess.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class TaskBootstrap
{
    public static IServiceCollection RegisterTaskDependencies(this IServiceCollection services)
    {
        return services
            .AddRepositories()
            .AddApplication()
            .AddMapping()
            .AddValidators();
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<ITasksGuestService, TasksGuestService>()
            .AddScoped<ITasksOwnerService, TasksOwnerService>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ITasksRepository, TasksRepository>();
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services
        .AddAutoMapper(
            typeof(TaskEntityMappingProfile).Assembly,
            typeof(TaskDtoOwnerMappingProfile).Assembly,
            typeof(TaskDtoGuestMappingProfile).Assembly);
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<AddTaskDto>, AddTaskDtoValidator>()
            .AddScoped<IValidator<UpdateTaskDto>, UpdateTaskDtoValidator>()
            .AddScoped<IValidator<TaskFilterDto>, TaskFilterDtoValidator>();
    }
}
