using EMS.Api.Helpers.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace EMS.Api.Helpers;
public class ExceptionHandlingMiddleware 
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, ex.Message);
            await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning(ex, ex.Message);
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning(ex, ex.Message);
            await WriteResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (ConflictException ex)
        {
            logger.LogWarning(ex, ex.Message);
            await WriteResponse(context, StatusCodes.Status409Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error");
            await WriteResponse(context, StatusCodes.Status500InternalServerError,
                "Internal server error");
        }
    }

    private static async Task WriteResponse(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            error = message
        });
    }
}
