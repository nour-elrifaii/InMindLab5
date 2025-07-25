using Lab5.Application.Mappers;
using Microsoft.EntityFrameworkCore;
using Lab5.Persistence.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Lab5.Application.Validators;
using Lab5.Application.Mappers;
using Lab5.Infrastructure;
using Serilog;
using Lab5.Infrastructure.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithClientIp()
    .Enrich.WithEnvironmentUserName()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHealthChecks();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<StudentValidator>();
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UniversityContext>(options=>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(UniversityMappingProfile).Assembly);
builder.Services.AddScoped<ObjectMapperService>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"),tags: new[] { "database" })
    .AddCheck<StudentCheck>("student check");
builder.Services.AddHealthChecksUI().AddInMemoryStorage();



var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});
app.MapHealthChecksUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();



app.Run();

