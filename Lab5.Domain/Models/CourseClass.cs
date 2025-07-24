using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
namespace Lab5.Domain.Models;

public class CourseClass
{
    public int CourseClassId { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    
    public Teacher Teacher { get; set; }
    public Course Course { get; set; }
    [JsonIgnore]
    public ICollection<Student>? RegStudents { get; set; }
}