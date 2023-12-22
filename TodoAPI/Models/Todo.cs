using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
    }
}
