using API.Requests.Courses;
using Application.Common.Interfaces;
using Application.Common.Utilities;
using Application.Courses.Dtos;
using Application.Courses.Commands.CreateCourse;
using Application.Courses.Commands.CreateLecture;
using Application.Courses.Commands.CreateSection;
using Application.Courses.Commands.UploadLectureVideo;
using Application.Courses.Queries.GetAllCourses;
using Application.Courses.Queries.GetCourseById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace API.Controllers;

public class CoursesController(ISender sender, ICurrentUser currentUser) : BaseApiController
{
    [HttpGet]
    [OutputCache(PolicyName = "CoursePolicy", VaryByQueryKeys = ["pageNumber", "pageSize", "categoriesIds", "levels"])]
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
        Collection<string>? levels = null;
        if (courseFiltersRequest.Levels is not null && !Collection<string>.TryParse(courseFiltersRequest.Levels, null, out levels))
        {
            return BadRequest("Level should be comma separated value like 'beginner,advanced'");
        }
        var query = new GetAllCoursesQuery(request.PageNumber, request.PageSize, currentUser.UserId, categoriesIds, levels);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpGet("{id:guid}")]
    [OutputCache(PolicyName = "CoursesPolicy", VaryByRouteValueNames = ["id"])]
    public async Task<ActionResult<CourseDto>> GetCourseById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCourseByIdQuery(id, currentUser.UserId);
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
