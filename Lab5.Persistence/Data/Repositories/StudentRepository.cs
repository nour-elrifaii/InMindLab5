using Lab5.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Persistence.Data.Repositories;

public class StudentRepository : IRepository<Student>
{
    private readonly UniversityContext _context;

    public StudentRepository(UniversityContext context)
    {
        _context = context;
    }
    
    
    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }
    
    
    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student)
    {
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }

    
}