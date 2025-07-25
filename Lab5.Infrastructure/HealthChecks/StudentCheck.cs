
using Lab5.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lab5.Infrastructure;

public class StudentCheck : IHealthCheck
{
    private readonly UniversityContext _context;
    public StudentCheck(UniversityContext context)=> _context = context;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var student = await _context.Students.AnyAsync(cancellationToken);
        return student ? HealthCheckResult.Healthy("students present") : HealthCheckResult.Unhealthy("no students present");
    }
}