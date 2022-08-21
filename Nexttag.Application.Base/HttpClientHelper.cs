using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Nexttag.Application.Base.Exceptions;
using Nexttag.Application.Base.Http;

namespace Nexttag.Application.Base;

    public class HttpClientHelper
    {

        IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _deserializeOption;
        public HttpClientHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _deserializeOption = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<Response> GetHttpAPI<Response>(string clientName, string path, NameValueCollection queryString = null, AuthenticationHeaderValue authenticationHeader = null, IDictionary<string, string> headers = null)
        {
            var context = HttpContextBuilder
                .CreateHttpGetContext<Response>(clientName, path)
                .SetQueryString(queryString)
                .SetAuthentication(authenticationHeader)
                .SetHeaders(headers);

            return await GetHttpAPI(context);
        }

        public async Task<Response> PostHttpAPI<Request, Response>(string clientName, string path, Request request, NameValueCollection queryString = null, AuthenticationHeaderValue authenticationHeader = null, IDictionary<string, string> headers = null)
        {
            var context = HttpContextBuilder
               .CreateHttpCallContext<Request, Response>(clientName, path, request);

            context
               .SetQueryString(queryString)
               .SetAuthentication(authenticationHeader)
               .SetHeaders(headers);

            return await PostHttpAPI(context);
        }

        public async Task<Response> PatchHttpAPI<Request, Response>(string clientName, string path, Request request, NameValueCollection queryString = null, AuthenticationHeaderValue authenticationHeader = null, IDictionary<string, string> headers = null)
        {
            var context = HttpContextBuilder
               .CreateHttpCallContext<Request, Response>(clientName, path, request);

            context
               .SetQueryString(queryString)
               .SetAuthentication(authenticationHeader)
               .SetHeaders(headers);

            return await PatchHttpAPI(context);
        }

        public async Task<Response> PutHttpAPI<Request, Response>(string clientName, string path, Request request, NameValueCollection queryString = null, AuthenticationHeaderValue authenticationHeader = null, IDictionary<string, string> headers = null)
        {
            var context = HttpContextBuilder
               .CreateHttpCallContext<Request, Response>(clientName, path, request);

            context
               .SetQueryString(queryString)
               .SetAuthentication(authenticationHeader)
               .SetHeaders(headers);

            return await PutHttpAPI(context);
        }

        public async Task<Response> DeleteHttpAPI<Response>(string clientName, string path, NameValueCollection queryString = null, AuthenticationHeaderValue authenticationHeader = null, IDictionary<string, string> headers = null)
        {
            var context = HttpContextBuilder
               .CreateHttpGetContext<Response>(clientName, path)
               .SetQueryString(queryString)
               .SetAuthentication(authenticationHeader)
               .SetHeaders(headers);

            return await DeleteHttpAPI(context);
        }

        private async Task<Response> SerializeResponse<Response>(HttpResponseMessage httpResponse, HttpGetContext<Response> context)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                var message = string.Empty;
                var rawResponse = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var errorResponse = default(ErrorMessage);
                try
                {
                    errorResponse = JsonSerializer.Deserialize<ErrorMessage>(
                        rawResponse,
                        _deserializeOption
                    );
                }
                catch (Exception) { }
                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        message = errorResponse?.Message ?? $"Endpoit não encontrado {httpResponse.RequestMessage.RequestUri}[{httpResponse.RequestMessage.Method}]: {rawResponse}";
                        throw new NotFoundException(message);
                    case HttpStatusCode.BadRequest:
                        message = errorResponse?.Message ?? $"Campo com algun problema no: {rawResponse}";
                        throw new BadRequestException(message, errorResponse.Fields ?? new List<Field>(0));
                    case HttpStatusCode.PreconditionRequired:
                        message = errorResponse?.Message ?? $"Necessário uma condição especifica para executar essa ação: {rawResponse}";
                        throw new PreconditionRequiredException(message);
                    case HttpStatusCode.PreconditionFailed:
                        message = errorResponse?.Message ?? $"Necessário uma condição especifica para executar essa ação: {rawResponse}";
                        throw new PreconditionFailedException(message);
                    default:
                        throw new Exception($"Error {httpResponse.StatusCode} on {httpResponse.RequestMessage.RequestUri}[{httpResponse.RequestMessage.Method}]: \n {rawResponse}");
                }
            }
            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                return default(Response);

            var responseStream = httpResponse.Content.ReadAsStream();
            context.Response = await JsonSerializer.DeserializeAsync<Response>(responseStream, context.ResponseDeserializeOption ?? _deserializeOption);
            return context.Response;
        }

        public async Task<Response> GetHttpAPI<Response>(HttpGetContext<Response> httpCallContext)
        {
            var httpClient = GetHttpClient(httpCallContext.ClientName, httpCallContext.AuthenticationHeader, httpCallContext.Headers);
            var httpResponse = await httpClient.GetAsync(GetParsedUrl(httpCallContext.Path, httpCallContext.QueryString));
            return await SerializeResponse(httpResponse, httpCallContext);
        }

        public async Task<Response> PostHttpAPI<Request, Response>(HttpCallContext<Request, Response> httpPostContext)
        {
            var httpClient = GetHttpClient(httpPostContext.ClientName, httpPostContext.AuthenticationHeader, httpPostContext.Headers);
            var httpResponse = await httpClient.PostAsync(GetParsedUrl(httpPostContext.Path, httpPostContext.QueryString), httpPostContext.Request.ToHttpContent(httpPostContext.RequestSerializeOption));
            return await SerializeResponse(httpResponse, httpPostContext);
        }

        public async Task<Response> PatchHttpAPI<Request, Response>(HttpCallContext<Request, Response> httpPatchContext)
        {
            var httpClient = GetHttpClient(httpPatchContext.ClientName, httpPatchContext.AuthenticationHeader, httpPatchContext.Headers);
            var httpResponse = await httpClient.PatchAsync(GetParsedUrl(httpPatchContext.Path, httpPatchContext.QueryString), httpPatchContext.Request.ToHttpContent(httpPatchContext.RequestSerializeOption));
            return await SerializeResponse(httpResponse, httpPatchContext);
        }

        public async Task<Response> PutHttpAPI<Request, Response>(HttpCallContext<Request, Response> httpPutContext)
        {
            var httpClient = GetHttpClient(httpPutContext.ClientName, httpPutContext.AuthenticationHeader, httpPutContext.Headers);
            var httpResponse = await httpClient.PutAsync(GetParsedUrl(httpPutContext.Path, httpPutContext.QueryString), httpPutContext.Request.ToHttpContent(httpPutContext.RequestSerializeOption));
            return await SerializeResponse(httpResponse, httpPutContext);
        }

        public async Task<Response> DeleteHttpAPI<Response>(HttpGetContext<Response> httpPatchContext)
        {
            var httpClient = GetHttpClient(httpPatchContext.ClientName, httpPatchContext.AuthenticationHeader, httpPatchContext.Headers);
            var httpResponse = await httpClient.DeleteAsync(GetParsedUrl(httpPatchContext.Path, httpPatchContext.QueryString));
            return await SerializeResponse(httpResponse, httpPatchContext);
        }

        private HttpClient GetHttpClient(string clientName, AuthenticationHeaderValue authenticationHeader, IDictionary<string, string> headers)
        {
            var httpClient = _clientFactory.CreateClient(clientName);
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return httpClient;
        }

        private string GetParsedUrl(string path, NameValueCollection queryString)
        {
            var parsedQueryString = path;
            if (queryString != null && queryString.Count > 0)
            {
                foreach (var key in queryString.AllKeys)
                {
                    parsedQueryString = QueryHelpers.AddQueryString(path, key, queryString[key]);
                }
            }
            return parsedQueryString;
        }
    }