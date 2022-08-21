using System.Dynamic;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nexttag.Communication.Email.Services;

public class SendMailService : ISendMailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly string _fixedTo;
    private readonly INotificationAllowlist<SendMailService> _notificationWhitelist;

    public SendMailService(string apiKey, string fromEmail, string fromName, string fixedTo,
        INotificationAllowlist<SendMailService> notificationWhitelist)
    {
        _apiKey = apiKey;
        _fromEmail = fromEmail;
        _fromName = fromName;
        _fixedTo = fixedTo;
        _notificationWhitelist = notificationWhitelist;
    }

    /// <summary>
    /// Envia um email com template cadastrado no sendgrid.
    /// </summary>
    /// <param name="email">Email do destinatario</param>
    /// <param name="templateId">id do template</param>
    /// <param name="substitutions"> dicionario com valores a serem substituidos </param>
    /// <returns></returns>
    public async Task<HttpStatusCode> SendByTemplate(string email, string templateId,
        Dictionary<string, string> substitutions = null)
    {
        var recepient = _notificationWhitelist.VerifyAndGetRecipient(email);
        if (string.IsNullOrEmpty(recepient))
        {
            return string.IsNullOrEmpty(email) ? HttpStatusCode.BadRequest : HttpStatusCode.MethodNotAllowed;
        }

        var client = new SendGridClient(_apiKey);
        var from = MailHelper.StringToEmailAddress(_fromEmail);
        from.Name = _fromName;
        var to = MailHelper.StringToEmailAddress(recepient);
        var msg = new SendGridMessage();
        var eo = new ExpandoObject();

        foreach (var sub in substitutions)
        {
            eo.TryAdd(sub.Key, sub.Value);
        }

        msg.SetTemplateData(eo);
        msg.SetFrom(from);
        msg.SetTemplateId(templateId);
        msg.AddTo(to);

        var response = await client.SendEmailAsync(msg);

        return response.StatusCode;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email">email do destinatario</param>
    /// <param name="templateId">id do template de email</param>
    /// <param name="template"></param>
    /// <returns></returns>
    public async Task<HttpStatusCode> SendByTemplate(string email, string templateId, object template = null)
    {
        var recepient = _notificationWhitelist.VerifyAndGetRecipient(email);
        if (string.IsNullOrEmpty(recepient))
        {
            return string.IsNullOrEmpty(email) ? HttpStatusCode.BadRequest : HttpStatusCode.MethodNotAllowed;
        }

        var client = new SendGridClient(_apiKey);
        var from = MailHelper.StringToEmailAddress(_fromEmail);
        from.Name = _fromName;
        var to = MailHelper.StringToEmailAddress(recepient);
        var msg = new SendGridMessage();
        msg.SetFrom(from);
        msg.SetTemplateId(templateId);
        msg.AddTo(to);

        if (template != null)
            msg.SetTemplateData(template);
        var response = await client.SendEmailAsync(msg);

        return response.StatusCode;
    }
}