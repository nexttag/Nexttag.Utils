using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Nexttag.Application.Base.Http;

public class HttpCallContext<In, Out> : HttpGetContext<Out>
{
    public HttpCallContext(string clientName, string path, In request, NameValueCollection queryString, IDictionary<string, string> headers, AuthenticationHeaderValue authenticationHeader, JsonSerializerOptions requestSeserializeOption = null, JsonSerializerOptions responseDeserializeOption = null)
        : base(clientName, path, queryString, headers, authenticationHeader, responseDeserializeOption)
    {
        RequestSerializeOption = requestSeserializeOption;
        Request = request;
    }

    public JsonSerializerOptions RequestSerializeOption { get; internal set; }
    public In Request { get; internal set; }

}