using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Domain.Models;
public class Course
{
    public int CourseID { get; set; }
    public string CourseName { get; set; }=string.Empty;
    public int TeacherId { get; set; }
    public ICollection<CourseClass>? Classes { get; set; }
}