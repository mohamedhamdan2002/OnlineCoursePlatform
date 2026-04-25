using API.Requests.Courses;
using Application.Common.Utilities;
using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Commands.CreateLecture;
using Application.Features.Courses.Commands.CreateSection;
using Application.Features.Courses.Commands.UploadLectureVideo;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Queries.GetAllCourses;
using Application.Features.Courses.Queries.GetCourseById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CoursesController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PageList<CourseDto>>> GetAllCourses([FromQuery] CourseFiltersRequest courseFiltersRequest ,[FromQuery] CoursePageRequest request, CancellationToken cancellationToken)
    {
        if (request.PageNumber <= 0)
        {
            return BadRequest("PageNumber must be greater than 0");
        }

        if (request.PageSize <= 0 || request.PageSize > 100)
        {
            return BadRequest("PageSize must be between 1 and 100");
        }
        GuidCollection? categoriesIds = null;
        if (courseFiltersRequest.CategoriesIds is not null && !GuidCollection.TryParse(courseFiltersRequest.CategoriesIds, null, out categoriesIds))
        {
            return BadRequest("CategoriesIds should be comma separated value like '0000-0000-0000-0000,0000-0000-0000-0000,0000-0000-0000-0000'");
        }
        var query = new GetAllCoursesQuery(request.PageNumber, request.PageSize, categoriesIds);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromForm] CreateCourseRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCourseCommand(request.CategoryId ,request.Title, request.Description, request.Price, request.Level, request.Image, request.InstructorId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => StatusCode(StatusCodes.Status201Created, result.Data));
    }

    [HttpPost("{courseId:guid}/sections")]
    public async Task<ActionResult<SectionDto>> CreateSection(Guid courseId, CreateSectionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSectionCommand(courseId, request.Title);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => StatusCode(StatusCodes.Status201Created, result.Data));
    }


    [HttpPost("/api/sections/{sectionId:guid}/lectures")]
    public async Task<ActionResult<SectionDto>> CreateLecture(Guid sectionId, CreateLectureRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateLectureCommand(sectionId, request.Title);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => StatusCode(StatusCodes.Status201Created, result.Data));
    }

    [HttpPost("/api/lectures/{lectureId:guid}/video")]
    public async Task<IActionResult> UploadLectureVideo(Guid lectureId, [FromForm] UploadLectureVideoRequest request, CancellationToken cancellationToken)
    {
        var command = new UploadLectureVideoCommand(lectureId, request.VideoFile);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => Accepted());
    }
}
