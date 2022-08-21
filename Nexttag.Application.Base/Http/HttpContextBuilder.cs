using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Nexttag.Application.Base.Http;

    public static class HttpContextBuilder
    {
        public static HttpGetContext<Out> CreateHttpGetContext<Out>(string clientName, string path)
        {
            return new HttpGetContext<Out>(clientName, path, new NameValueCollection(), null, null);
        }

        public static HttpCallContext<In, Out> CreateHttpCallContext<In, Out>(string clientName, string path, In request)
        {
            return new HttpCallContext<In, Out>(clientName, path, request, new NameValueCollection(), null, null);
        }

        public static HttpGetContext<Out> SetQueryString<Out>(this HttpGetContext<Out> context, NameValueCollection queryString)
        {
            if (queryString != null)
            {
                foreach (var key in queryString.AllKeys)
                {
                    context.Path = QueryHelpers.AddQueryString(context.Path, key, queryString[key]);
                }
            }
            return context;
        }

        public static HttpGetContext<Out> AddHeader<Out>(this HttpGetContext<Out> context, string key, string value)
        {
            if (context.Headers == null)
            {
                context.Headers = new Dictionary<string, string>();
            }
            context.Headers[key] = value;
            return context;
        }

        public static HttpGetContext<Out> AddAuthentication<Out>(this HttpGetContext<Out> context, string scheme, string token)
        {
            context.AuthenticationHeader = new AuthenticationHeaderValue(scheme, token);
            return context;
        }

        public static HttpGetContext<Out> AddQueryString<Out>(this HttpGetContext<Out> context, string key, string value)
        {
            context.Path = QueryHelpers.AddQueryString(context.Path, key, value);
            return context;
        }

        public static HttpGetContext<Out> SetReponseDeserialize<Out>(this HttpGetContext<Out> context, JsonSerializerOptions jsonSerializerOptions)
        {
            context.ResponseDeserializeOption = jsonSerializerOptions;
            return context;
        }
        internal static HttpGetContext<Out> SetHeaders<Out>(this HttpGetContext<Out> context, IDictionary<string, string> headers)
        {
            context.Headers = headers;
            return context;
        }

        internal static HttpGetContext<Out> SetAuthentication<Out>(this HttpGetContext<Out> context, AuthenticationHeaderValue authenticationHeader)
        {
            context.AuthenticationHeader = authenticationHeader;
            return context;
        }

        public static HttpCallContext<In, Out> SetRequestSerialize<In, Out>(this HttpCallContext<In, Out> context, JsonSerializerOptions jsonSerializerOptions)
        {
            context.RequestSerializeOption = jsonSerializerOptions;

            return context;
        }


        public static HttpCallContext<In, Out> SetRequest<In, Out>(this HttpCallContext<In, Out> context, In request, JsonSerializerOptions jsonSerializerOptions = null)
        {
            context.Request = request;
            if (jsonSerializerOptions != null)
            {
                context.RequestSerializeOption = jsonSerializerOptions;
            }

            return context;
        }
    }