using System.Globalization;
using Lab5.Application.Mappers;
using Microsoft.EntityFrameworkCore;
using Lab5.Persistence.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Lab5.Application.Validators;
using Lab5.Application.Mappers;
using Lab5.Application.Queries;
using Lab5.Domain.Models;
using Lab5.Infrastructure;
using Serilog;
using Lab5.Infrastructure.Middleware;
using Lab5.Persistence.Data.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Hangfire;
using Hangfire.MemoryStorage;
using Lab5.Application.BackgroundJobs;
using Lab5.Application.Services;
using Lab5.Infrastructure.Services;

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
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
// ran this to make sure it was working /students/tesssst?culture=fr
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new List<CultureInfo>
    {
        new CultureInfo("en"),
        new CultureInfo("fr"),

    };
    options.DefaultRequestCulture = new RequestCulture("fr");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllStudentsQuery).Assembly));
builder.Services.AddScoped<IRepository<Student>, StudentRepository>();
builder.Services.AddScoped<IRepository<Teacher>, TeacherRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Lab5:";
});
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMemoryStorage());

builder.Services.AddHangfireServer();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
    /*.AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<StudentValidator>();
    });*/
    builder.Services
        .AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UniversityContext>(options=>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(UniversityMappingProfile).Assembly);
builder.Services.AddScoped<ObjectMapperService>();
//builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"),tags: new[] { "database" })
    .AddCheck<StudentCheck>("student check");
builder.Services.AddHealthChecksUI().AddInMemoryStorage();




var app = builder.Build();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
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
app.UseHangfireDashboard("/hangfire");
app.MapControllers();



app.Run();

