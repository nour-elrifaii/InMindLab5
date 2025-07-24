using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
namespace Lab5.Domain.Models
{
    public class Student
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public ICollection<CourseClass>? RegClasses { get; set; }
    }
}