namespace Lab5.Application.BackgroundJobs;

public interface IBackgroundJobService
{ 
    void SendConfirmationEmail(string studentName, string email);
}
