namespace ElkoodProject.Application.Contracts.Task.Guest.Dtos;

using System.Collections.Generic;

public class TaskListItemsGuestDto
{
    public IList<TaskGuestDto>? Items { get; set; }
}
