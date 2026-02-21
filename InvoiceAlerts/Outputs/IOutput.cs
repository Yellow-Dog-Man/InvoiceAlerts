using FluentResults;

namespace InvoiceAlerts.Outputs;

public interface IOutput
{
    public string Name { get; }
    public string Description { get; }
    public bool Enabled { get; }

    public Result Send();
}
