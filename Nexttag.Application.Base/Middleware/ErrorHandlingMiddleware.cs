using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nexttag.Application.Base.Exceptions;
using Nexttag.Application.Base.Http;

namespace Nexttag.Application.Base.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        var code = HttpStatusCode.InternalServerError;
        string? message = null;
        IList<Field>? fields = null;

        switch (exception)
        {
            case NotFoundException nf:
                code = HttpStatusCode.NotFound;
                message = nf.Message;
                break;
            case BadRequestException br:
                code = HttpStatusCode.BadRequest;
                message = br.Message;
                fields = br.InvalidFields.ToList();
                break;
            case NotImplementedException:
                code = HttpStatusCode.NotImplemented;
                break;
            case PreconditionRequiredException pr:
                code = HttpStatusCode.PreconditionRequired;
                message = pr.Message;
                break;
            case ForbiddenException fb:
                code = HttpStatusCode.Forbidden;
                message = fb.Message;
                break;
            case PreconditionFailedException pf:
                code = HttpStatusCode.PreconditionFailed;
                message = pf.Message;
                break;
            default:
                logger.LogError(exception, exception.Message);
                break;
        }

        var result = JsonSerializer.Serialize(
            new ErrorMessage()
            {
                Message = message,
                Code = code,
                Fields = fields
            },
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}