using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexttag.Communication.Email.Services;

namespace Nexttag.Communication.Email;

public static class ServiceCollectionExtensions
{
    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(INotificationAllowlist<SendMailService>),
            notification => new NotificationAllowlist<SendMailService>(
                configuration.GetSection("Notification:Email:AllowList").Get<string[]>(),
                configuration.GetSection("Notification:Email:Override").Get<string>()
            ));

        var notificationWhitelist = services
            .BuildServiceProvider().GetService<INotificationAllowlist<SendMailService>>();

        var sendMail = new SendMailService(configuration["SendGrid:Key"],
            configuration["SendGrid:From:Email"],
            configuration["SendGrid:From:Name"],
            configuration["SendGrid:To:Email"],
            notificationWhitelist);

        services.AddTransient<ISendMailService, SendMailService>(x => sendMail);
        
    }
}