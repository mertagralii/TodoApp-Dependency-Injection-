using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required] // Zorunlu olduğunu belirtiyoruz.
        public string JobName { get; set; }

        public bool IsApproved { get; set; }
    }
}
