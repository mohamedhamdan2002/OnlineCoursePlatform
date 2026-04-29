using Application.Features.Enrollments.Dtos;
using Application.Features.Enrollments.Queries.GetMyEnrollments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class EnrollmentsController(ISender sender) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<List<EnrollmentDto>>> GetMyEnrollments(CancellationToken cancellationToken)
    {
        var query = new GetMyEnrollmentsQuery();
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }
}
