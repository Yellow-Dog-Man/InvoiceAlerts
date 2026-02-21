using InvoiceAlerts;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<EmailManager>();
builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();
