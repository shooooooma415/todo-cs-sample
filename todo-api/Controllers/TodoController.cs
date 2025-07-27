using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(TodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(TodoCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title is required");
            }

            try
            {
                var todo = await _todoService.CreateTodoAsync(request);
                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(Guid id)
        {
            try
            {
                var todo = await _todoService.GetTodoByIdAsync(id);

                if (todo == null)
                {
                    return NotFound();
                }

                return todo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting todo with id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos([FromQuery] string? status)
        {
            try
            {
                IEnumerable<Todo> todos;
                
                if (!string.IsNullOrEmpty(status))
                {
                    todos = await _todoService.GetTodosByStatusAsync(status);
                }
                else
                {
                    todos = await _todoService.GetAllTodosAsync();
                }

                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting todos");
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 