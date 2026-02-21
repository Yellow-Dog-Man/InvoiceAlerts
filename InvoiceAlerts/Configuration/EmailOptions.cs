

namespace InvoiceAlerts.Configuration;

public class EmailOptions
{
    public const int EMAIL_PORT = 993;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = EMAIL_PORT;

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool SSL { get; set; } = true;
}
