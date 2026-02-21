using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceAlerts.Configuration;

public class GithubOptions
{
    public string Token { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Repository { get; set; } = string.Empty;
}
