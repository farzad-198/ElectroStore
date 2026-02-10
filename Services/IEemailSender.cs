namespace ElectroStore.Services
{
    public interface IEemailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlMessage);

    }
}
