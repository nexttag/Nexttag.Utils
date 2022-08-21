using System.Net;


namespace Nexttag.Application.Base.Http;

public class ErrorMessage
{
    public string Message { get; set; }
    public HttpStatusCode Code { get; set; }
    public IList<Field> Fields { get; set; }
}