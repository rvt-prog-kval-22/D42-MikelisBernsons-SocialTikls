namespace BredWeb.Models
{
    public record EmailConfigViewModel(
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
