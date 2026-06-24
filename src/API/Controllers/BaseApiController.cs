using Domain.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected ActionResult HandleError(Error error)
    {
        if (error is null || error.StatusCode == StatusCodes.Status200OK)
            throw new ArgumentNullException(nameof(error));
        return Problem(statusCode: error.StatusCode, title: error.Message);
    }

    protected ActionResult HandleResult(Result result, Func<ActionResult> onReturn)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        if (result.IsFailure)
            return HandleError(result.Error);

        return onReturn();
    }
}
