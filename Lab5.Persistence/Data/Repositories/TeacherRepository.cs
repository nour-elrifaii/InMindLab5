using Lab5.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Persistence.Data.Repositories;

public class TeacherRepository : IRepository<Teacher>
{
    private readonly UniversityContext _context;

    public TeacherRepository(UniversityContext context)
    {
        _context = context;
    }
    
    public async Task<List<Teacher>> GetAllAsync()
    {
        return await _context.Teachers.ToListAsync();
    }

    public async Task AddAsync(Teacher teacher)
    {
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Teacher teacher)
    {
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
    }
}