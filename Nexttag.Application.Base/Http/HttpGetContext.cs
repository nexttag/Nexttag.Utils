using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Nexttag.Application.Base.Http;

public class HttpGetContext<Out>
{
    public HttpGetContext(string clientName, string path, NameValueCollection queryString, IDictionary<string, string> headers, AuthenticationHeaderValue authenticationHeader, JsonSerializerOptions responseDeserializeOption = null)
    {
        ClientName = clientName;
        Path = path;
        QueryString = queryString;
        Headers= headers;
        AuthenticationHeader = authenticationHeader;
        ResponseDeserializeOption = responseDeserializeOption;
        Response = default(Out);
    }

    public JsonSerializerOptions ResponseDeserializeOption { get; internal set; }
    public string ClientName { get; internal set; }
    public string Path { get; internal set; }
    public NameValueCollection QueryString { get; internal set; }
    public IDictionary<string, string> Headers { get; internal set; }
    public AuthenticationHeaderValue AuthenticationHeader { get; internal set; }
    public Out Response { get; internal set; }

        
}