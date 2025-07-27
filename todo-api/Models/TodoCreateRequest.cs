using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoCreateRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = "medium";
    }
} 