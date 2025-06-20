using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Handlers;
using TodoApp.Application.Queries;

namespace TodoApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly GetAllTodosQueryHandler _getAllTodosQueryHandler;

    public TodosController(GetAllTodosQueryHandler getAllTodosQueryHandler)
    {
        _getAllTodosQueryHandler = getAllTodosQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
    {
        var query = new GetAllTodosQuery();
        var result = await _getAllTodosQueryHandler.Handle(query);
        return Ok(result);
    }
} 