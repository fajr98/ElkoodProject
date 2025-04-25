namespace ElkoodProject.Task.DataAccess;

using System;
using ElkoodProject.Tasks.DataAccess.Entities;
using ElkoodProject.Tasks.DataAccess.ModelBuilders;
using Microsoft.EntityFrameworkCore;

public class TasksDbContext : DbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }

    public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskEntityTypeConfiguration).Assembly);
    }
}
