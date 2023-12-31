using Microsoft.AspNetCore.Mvc;
using SistemaVendasApi.Validation;

namespace SistemaVendasApi.Controllers.Models;

public class ExceptionResponse()
{
    public static void Error(ref Response response, Exception ex, ILogger logger)
    {
        response.Validation = new ValidateProcess();
        response.Validation.Add(new ModelValid()
        {
            Type = ValidType.Error,
            Message = ex.Message
        });
        logger.LogDebug("response",[response]);
        logger.LogDebug("ex", [ex]);
        logger.LogError("Exception",[ex]);
        logger.LogTrace("Return Error");
    }
    public static void Error(ref Response response, string message, ILogger logger)
    {
        response.Validation = new ValidateProcess();
        response.Validation.Add(new ModelValid()
        {
            Type = ValidType.Error,
            Message = message
        });
        logger.LogDebug("response",[response]);
        logger.LogTrace("Return Error");
    }
    public static void Ok(ref Response response, object data, ILogger logger)
    {
        logger.LogDebug("data", [data]);
        response.Validation = new ValidateProcess();
        response.Validation.Add(new ModelValid()
        {
            Type = ValidType.Info,
            Message = "Return OK"
        });
        response.Data = data;
        logger.LogDebug("response", [response]);
        logger.LogTrace("Return OK");
    }

    public static void Ok(ref Response response, string message, ILogger logger)
    {
        logger.LogDebug("message", [message]);
        response.Validation = new ValidateProcess();
        response.Validation.Add(new ModelValid()
        {
            Type = ValidType.Info,
            Message = message
        });
        logger.LogDebug("response", [response]);
        logger.LogTrace("Return OK");
    }

    public static void Warning(ref Response response, string message, ILogger logger)
    {
        logger.LogDebug("message", [message]);
        response.Validation = new ValidateProcess();
        response.Validation.Add(new ModelValid()
        {
            Type = ValidType.Warning,
            Message = message
        });
        logger.LogDebug("response", [response]);
        logger.LogTrace("Return Warning");
    }
}