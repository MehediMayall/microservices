using Bookify.Application;
using Bookify.Domain;

namespace Bookify.Infrastructure;

internal sealed class EmailService: IEmailService
{
    public Task SendAsync(Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}