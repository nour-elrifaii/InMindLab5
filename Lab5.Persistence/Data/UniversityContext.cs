using Lab5.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;

namespace Lab5.Persistence.Data;

public class UniversityContext : DbContext
{
    public UniversityContext(DbContextOptions<UniversityContext> options) 
        : base(options)
    {
        
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseClass> CourseClasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseClass>()
            .HasMany(c => c.RegStudents)
            .WithMany(s => s.RegClasses)
            .UsingEntity<Dictionary<string, object>>(
                "CourseClassesRegistration",
                j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
                j => j.HasOne<CourseClass>().WithMany().HasForeignKey("CourseClassId")
            );
    }

}