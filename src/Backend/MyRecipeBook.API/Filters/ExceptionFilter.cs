using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookException)
            HandleProjectException(context);
        else
            ThrowUnknowException(context);
    }

    public static void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException exception)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
        }
    }

    public static void ThrowUnknowException(ExceptionContext context)
    {
        Console.WriteLine(context.Exception.Message);
        context.HttpContext.Response.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOW_ERROR));
    }
}