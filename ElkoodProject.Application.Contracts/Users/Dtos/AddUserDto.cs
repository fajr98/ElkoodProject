namespace ElkoodProject.Application.Contracts.Users.Dtos;

public class AddUserDto
{
    public string Email { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string? Mobile { get; set; }

    public string Password { get; set; } = default!;
}
