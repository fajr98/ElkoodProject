namespace ElkoodProject.Tasks.DataAccess.ModelBuilders;

using System;
using ElkoodProject.Tasks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskEntityTypeConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        builder.ToTable("Tasks", "tsk");

        builder.Property(a => a.Name)
            .IsRequired()
            .IsUnicode(true)
            .HasMaxLength(64);

        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(256);
    }
}
