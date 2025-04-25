// Copyright (c) Phinexes. All rights reserved.

namespace ElkoodProject.Controllers;

using ElkoodProject.Application.Contracts.Task.Guest.Contracts;
using ElkoodProject.Application.Contracts.Task.Guest.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1.0/tasks/guest")]
[Authorize(Policy = "OwnerRolePolicy")]
public class TaskGuestController : ControllerBase
{
    private readonly ITasksGuestService _tasksGuestService;

    public TaskGuestController(ITasksGuestService tasksGuestService)
    {
        _tasksGuestService = tasksGuestService;
    }

    [HttpGet]
    [Produces(typeof(TaskListItemsGuestDto))]
    public async Task<IActionResult> GetAllAsync([FromQuery] TaskFilterDto taskFilterDto, [FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        return Ok(await _tasksGuestService.GetAllAsync(taskFilterDto, pageIndex, pageSize));
    }
}
