using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Web.Microondas.Application.Exceptions;

namespace Web.Microondas.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logFilePath;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt");
        
        var logDirectory = Path.GetDirectoryName(_logFilePath);
        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory!);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        LogException(exception);

        var (statusCode, message) = exception switch
        {
            BusinessRuleException => (HttpStatusCode.BadRequest, exception.Message),
            ValidationException validationEx => (HttpStatusCode.BadRequest, FormatValidationErrors(validationEx)),
            _ => (HttpStatusCode.InternalServerError, "An internal server error occurred. Please try again later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            timestamp = DateTime.UtcNow
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    private void LogException(Exception exception)
    {
        var logEntry = $"""
            ============================================
            TIMESTAMP: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
            EXCEPTION: {exception.GetType().FullName}
            MESSAGE: {exception.Message}
            INNER EXCEPTION: {exception.InnerException?.Message ?? "None"}
            STACK TRACE:
            {exception.StackTrace}
            ============================================

            """;

        try
        {
            File.AppendAllText(_logFilePath, logEntry);
            Console.WriteLine($"Exception logged to: {_logFilePath}");
        }
        catch (Exception logEx)
        {
            Console.WriteLine($"Failed to log exception to {_logFilePath}: {logEx.Message}");
        }
    }

    private string FormatValidationErrors(ValidationException validationException)
    {
        var errors = string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage));
        return $"Validation failed: {errors}";
    }
}
