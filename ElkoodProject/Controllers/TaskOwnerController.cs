namespace ElkoodProject.Controllers;

using ElkoodProject.Application.Contracts.Task.Owner.Contracts;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1.0/tasks/owner")]
[Authorize(Policy = "OwnerRolePolicy")]
public class TaskOwnerController : ControllerBase
{
    private readonly ITasksOwnerService _tasksOwnerService;

    public TaskOwnerController(ITasksOwnerService tasksOwnerService)
    {
        _tasksOwnerService = tasksOwnerService;
    }

    [HttpGet]
    [Produces(typeof(TaskListItemsDto))]
    public async Task<IActionResult> GetAllAsync([FromQuery] TaskFilterDto taskFilterDto, [FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        return Ok(await _tasksOwnerService.GetAllAsync(taskFilterDto, pageIndex, pageSize));
    }


    [HttpPost]
    [Produces(typeof(TaskDto))]
    public async Task<IActionResult> AddAsync([FromBody] AddTaskDto addTaskDto)
    {
        return Ok(await _tasksOwnerService.AddAsync(addTaskDto));
    }

    [HttpPatch]
    [Produces(typeof(TaskDto))]
    public async Task<IActionResult> UpdateAsync([FromQuery] Guid id, [FromForm] UpdateTaskDto updateTaskDto)
    {
        return Ok(await _tasksOwnerService.UpdateAsync(id, updateTaskDto));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync([FromQuery] Guid id)
    {
        await _tasksOwnerService.RemoveAsync(id);
        return Ok();
    }
}
