using InvoiceAlerts.Configuration;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InvoiceAlerts;

public class EmailManager
{
    public ILogger<EmailManager> Log;


    private IOptions<EmailOptions> Options;
    private IOptions<InboxOptions> InboxOptions;
    private ImapClient Client;
    private OutputManager Outputs;
    public EmailManager(ILogger<EmailManager> log, IOptions<EmailOptions> options, IOptions<InboxOptions> inboxOptions, OutputManager outputs)
    {
        Log = log;
        Client = new ImapClient();
        Options = options;
        InboxOptions = inboxOptions;

        Outputs = outputs;
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

    private SearchQuery MakeQuery()
    {
        return SearchQuery.NotSeen;
    }

    private async Task ProcessFolder(IMailFolder inbox)
    {
        await inbox.OpenAsync(FolderAccess.ReadWrite);

        foreach (var messageId in await inbox.SearchAsync(MakeQuery()))
        {
            var message = await inbox.GetMessageAsync(messageId);
            try
            {
                await ProcessMessage(message);

                // If processing was successful mark as read, won't be included in the query
                await inbox.AddFlagsAsync(messageId, MessageFlags.Seen, false);
            }
            catch(Exception ex)
            {

            }
        }
    }

    private async Task ProcessMessage(MimeMessage message)
    {
        if (!await ShouldProcessMessage(message))
            return;
        
        var text = message.TextBody;
        var subject = message.Subject;

        // Is it an Invoice?
        // Check For Subject and Body containing "invoice"?
        // Check for PDF attachment?

        // If found send to outputs.
        Outputs.Output();
    }

    private async Task<bool> ShouldProcessMessage(MimeMessage message)
    {
        if (IsIgnoredSender(message.Sender))
            return false;

        return true;
    }

    private bool IsIgnoredSender(MailboxAddress? sender)
    {
        if (sender == null)
            return true;

        var options = InboxOptions.Value;

        if (options.IgnoredAddresses.Contains(sender.Address.ToLowerInvariant()))
            return true;

        return false;
    }

    
}
