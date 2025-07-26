using Lab5.Application.Services;

namespace Lab5.Infrastructure.Services;

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // simulate sending email 
            Console.WriteLine("Email To: " + to);
            Console.WriteLine("Subject: " + subject);
            Console.WriteLine("Body:\n" + body);

            // simulate delay
            await Task.Delay(500);
        }
    }