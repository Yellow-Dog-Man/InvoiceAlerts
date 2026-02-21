using InvoiceAlerts.Configuration;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;

namespace InvoiceAlerts;

public class EmailManager
{
    public ILogger<EmailManager> Log;


    private IOptions<EmailOptions> Options;
    private ImapClient Client;
    public EmailManager(ILogger<EmailManager> log, IOptions<EmailOptions> options)
    {
        Log = log;
        Client = new ImapClient();
        Options = options;
    }

    public async Task Pull()
    {
        try
        {
            var options = Options.Value; 

            await Client.ConnectAsync(options.Host, options.Port, true);
            await Client.AuthenticateAsync(options.Username, options.Password);

            await ProcessFolder(Client.Inbox);
           
        }
        catch (Exception ex)
        {
            /// TOOD
        }
        finally
        {
            if (Client.IsConnected)
                await Client.DisconnectAsync(true);
        }
    }

    private async Task ProcessFolder(IMailFolder inbox)
    {
        await inbox.OpenAsync(FolderAccess.ReadWrite);

        foreach (var messageId in await inbox.SearchAsync(MakeQuery()))
        {

        }

    }

    private SearchQuery MakeQuery()
    {
        return SearchQuery.NotSeen;
    }
}
