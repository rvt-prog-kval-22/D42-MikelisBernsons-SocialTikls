namespace BredWeb.Models
{
    public record EmailConfig(
        EmailConfigData Production,
        EmailConfigData Development
    );
    public record EmailConfigData(
        string SenderEmail,
        string SenderPassword,
        string From,
        string Client,
        int Port
    );
}
