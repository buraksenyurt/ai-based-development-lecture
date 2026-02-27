using CvApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CvApp.Api.Filters;

/// <summary>
/// Controller katmanında fırlatılan istisnaları yakalayıp uygun HTTP yanıtlarına dönüştürür.
/// </summary>
public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        context.Result = exception switch
        {
            NotFoundException notFound => BuildProblem(
                context,
                StatusCodes.Status404NotFound,
                "Kaynak Bulunamadı",
                notFound.Message),

            ArgumentNullException argNull => BuildProblem(
                context,
                StatusCodes.Status400BadRequest,
                "Geçersiz İstek",
                argNull.Message),

            ArgumentException argEx => BuildProblem(
                context,
                StatusCodes.Status400BadRequest,
                "Geçersiz İstek",
                argEx.Message),

            InvalidOperationException invalidOp => BuildProblem(
                context,
                StatusCodes.Status409Conflict,
                "İşlem Çakışması",
                invalidOp.Message),

            _ => BuildProblem(
                context,
                StatusCodes.Status500InternalServerError,
                "Sunucu Hatası",
                "Beklenmedik bir hata oluştu.")
        };

        var statusCode = ((ObjectResult)context.Result).StatusCode ?? 500;

        if (statusCode >= 500)
            _logger.LogError(exception, "İşlenmeyen hata oluştu: {Message}", exception.Message);
        else
            _logger.LogWarning(exception, "İstemci hatası: {Message}", exception.Message);

        context.ExceptionHandled = true;
    }

    private static ObjectResult BuildProblem(
        ExceptionContext context,
        int statusCode,
        string title,
        string detail)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.HttpContext.Request.Path
        };

        return new ObjectResult(problem) { StatusCode = statusCode };
    }
}
