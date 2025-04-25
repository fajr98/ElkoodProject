namespace ElkoodProject.Task.Application.Owner.Validators;

using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using FluentValidation;

public class TaskFilterDtoValidator : AbstractValidator<TaskFilterDto>
{
    public TaskFilterDtoValidator()
    {
        RuleFor(a => a.Name)
            .MaximumLength(64)
            .MinimumLength(3)
            .When(a => !string.IsNullOrEmpty(a.Name))
            .WithMessage("Invalid Name, it must be between 3 and 20 characters");


        RuleFor(a => a.DiedLineInHours)
            .InclusiveBetween(1, 24)
            .When(a => a.DiedLineInHours is not null)
            .WithMessage("Invalid DiedLineInHours, it must be between 1 and 24");

        RuleFor(a => a.Status)
            .IsInEnum()
            .When(a => a.Status is not null)
            .WithMessage("Invalid Status");

        RuleFor(a => a.Category)
            .IsInEnum()
            .When(a => a.Category is not null)
            .WithMessage("Invalid Category");

        RuleFor(a => a.Priority)
            .GreaterThanOrEqualTo(0)
            .When(a => a.Priority is not null)
            .WithMessage("Priority should not be null");
    }
}
