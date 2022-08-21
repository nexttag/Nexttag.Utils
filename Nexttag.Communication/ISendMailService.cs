using System.Net;

namespace Nexttag.Communication;

public interface ISendMailService
{
    Task<HttpStatusCode> SendByTemplate(string email, string templateId, Dictionary<string, string> substitutions = null);
    Task<HttpStatusCode> SendByTemplate(string email, string templateId, object template = null);
}