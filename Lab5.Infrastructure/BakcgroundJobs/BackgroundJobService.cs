using Hangfire;
using Lab5.Application.Services;
using Microsoft.Extensions.Logging;

namespace Lab5.Application.BackgroundJobs;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly ILogger<BackgroundJobService> _logger;
    private readonly IEmailService _emailService;

    public BackgroundJobService(ILogger<BackgroundJobService> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public void SendConfirmationEmail(string studentName, string email)
    {
        _logger.LogInformation("Preparing confirmation email for: {Email}", email);

        var subject = "Welcome to University!";
        var body = $"Hi {studentName},\n\n" +
                   "We're excited to have you on board. Your registration has been successfully completed.\n\n";

        _emailService.SendEmailAsync(email, subject, body);

        _logger.LogInformation("Confirmation email sent to: {Email}", email);
    }
}