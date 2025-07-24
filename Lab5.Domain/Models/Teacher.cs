using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
namespace Lab5.Domain.Models;


public class Teacher
{
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public ICollection<CourseClass>? Classes { get; set; }
}