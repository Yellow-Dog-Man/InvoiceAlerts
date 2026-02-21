namespace InvoiceAlerts.Configuration;

public class InboxOptions
{
    /// <summary>
    /// List of email addresses to ignore
    /// </summary>
    public List<string> IgnoredAddresses { get; set; } = new List<string>();
}
