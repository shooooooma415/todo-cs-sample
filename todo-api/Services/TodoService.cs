using Microsoft.Data.SqlClient;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService
    {
        private readonly string _connectionString;
        private readonly ILogger<TodoService> _logger;

        public TodoService(IConfiguration configuration, ILogger<TodoService> logger) //コンストラクタ
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<Todo> CreateTodoAsync(TodoCreateRequest request)
        {
            var sql = @"
                INSERT INTO Todos (Id, Title, Description, Status, Priority, DueDate, CreatedAt, UpdatedAt)
                OUTPUT INSERTED.Id, INSERTED.Title, INSERTED.Description, INSERTED.Status, INSERTED.Priority, INSERTED.DueDate, INSERTED.CreatedAt, INSERTED.UpdatedAt
                VALUES (NEWID(), @Title, @Description, @Status, @Priority, @DueDate, GETUTCDATE(), GETUTCDATE())";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Title", request.Title);
            command.Parameters.AddWithValue("@Description", (object?)request.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", "pending");
            command.Parameters.AddWithValue("@Priority", request.Priority);
            command.Parameters.AddWithValue("@DueDate", (object?)request.DueDate ?? DBNull.Value);

            _logger.LogInformation("Executing SQL: {Sql}", sql);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Todo
                {
                    Id = reader.GetGuid("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    Priority = reader.GetString("Priority"),
                    DueDate = reader.IsDBNull("DueDate") ? null : reader.GetDateTime("DueDate"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }

            throw new InvalidOperationException("Failed to create todo");
        }

        public async Task<Todo?> GetTodoByIdAsync(Guid id)
        {
            var sql = @"
                SELECT Id, Title, Description, Status, Priority, DueDate, CreatedAt, UpdatedAt
                FROM Todos
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            _logger.LogInformation("Executing SQL: {Sql}", sql);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Todo
                {
                    Id = reader.GetGuid("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    Priority = reader.GetString("Priority"),
                    DueDate = reader.IsDBNull("DueDate") ? null : reader.GetDateTime("DueDate"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }

            return null;
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            var todos = new List<Todo>();
            var sql = @"
                SELECT Id, Title, Description, Status, Priority, DueDate, CreatedAt, UpdatedAt
                FROM Todos
                ORDER BY CreatedAt DESC";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            _logger.LogInformation("Executing SQL: {Sql}", sql);
            
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetGuid("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    Priority = reader.GetString("Priority"),
                    DueDate = reader.IsDBNull("DueDate") ? null : reader.GetDateTime("DueDate"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                });
            }

            return todos;
        }

        public async Task<IEnumerable<Todo>> GetTodosByStatusAsync(string status)
        {
            var todos = new List<Todo>();
            var sql = @"
                SELECT Id, Title, Description, Status, Priority, DueDate, CreatedAt, UpdatedAt
                FROM Todos
                WHERE Status = @Status
                ORDER BY CreatedAt DESC";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Status", status);

            _logger.LogInformation("Executing SQL: {Sql}", sql);
            
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetGuid("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    Priority = reader.GetString("Priority"),
                    DueDate = reader.IsDBNull("DueDate") ? null : reader.GetDateTime("DueDate"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                });
            }

            return todos;
        }
    }
} 