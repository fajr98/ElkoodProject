namespace ElkoodProject.Task.Application.Owner.Validators;

using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using FluentValidation;

public class AddTaskDtoValidator : AbstractValidator<AddTaskDto>
{
    public AddTaskDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("Invalid Name, it must not be empty");

        RuleFor(a => a.Name)
            .MaximumLength(64)
            .WithMessage("Invalid Name, it must be between 3 and 20 characters");

        RuleFor(a => a.Name)
            .MinimumLength(3)
            .WithMessage("Invalid Name, it must be between 3 and 20 characters");

        RuleFor(a => a.Description)
            .NotEmpty()
            .WithMessage("Invalid Description, it must not be empty");

        RuleFor(a => a.Description)
            .MaximumLength(256)
            .WithMessage("Invalid Description, it must be between 20 and 256 characters");

        RuleFor(a => a.Description)
            .MinimumLength(20)
            .WithMessage("Invalid Description, it must be between 20 and 256 characters");

        RuleFor(a => a.DiedLineInHours)
            .NotEmpty()
            .WithMessage("Invalid DiedLineInHours,it must not be empty");

        RuleFor(a => a.DiedLineInHours)
            .InclusiveBetween(1, 24)
            .WithMessage("Invalid DiedLineInHours, it must be between 1 and 24");

        RuleFor(a => a.Status)
            .NotEmpty()
            .WithMessage("Invalid Status");

        RuleFor(a => a.Status)
            .IsInEnum()
            .WithMessage("Status must not be empty");

        RuleFor(a => a.Category)
            .NotEmpty()
            .WithMessage("Invalid Category");

        RuleFor(a => a.Category)
            .IsInEnum()
            .WithMessage("Category must not be empty");

        RuleFor(a => a.Priority)
            .NotEmpty()
            .WithMessage("Priority should not be null");

        RuleFor(a => a.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Priority should be greater than or equal to 0");
    }
}
