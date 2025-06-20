using TodoApp.Application.DTOs;
using TodoApp.Application.Queries;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Handlers;

public class GetAllTodosQueryHandler
{
    private readonly ITodoRepository _todoRepository;

    public GetAllTodosQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoDto>> Handle(GetAllTodosQuery query)
    {
        var todos = await _todoRepository.GetAllAsync();

        return todos.Select(todo => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            IsDone = todo.IsDone
        });
    }
} 