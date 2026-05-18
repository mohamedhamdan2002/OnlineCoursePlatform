using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Reviews.Dtos;
using Application.Reviews.Mappers;
using Domain.Common.Results;
using Domain.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler(IAppDbContext context, ICurrentUser currentUser, HybridCache cache) : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly HybridCache _cache = cache;

    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        var isCourseExist = await _context.Courses.AsNoTracking().Where(course => course.Id == command.CourseId).AnyAsync(cancellationToken);
        if (!isCourseExist)
            return Result.Fail<ReviewDto>(ApplicationErrors.CourseNotFound);

        var isStudentEnrolled = await _context.Enrollments.AsNoTracking().Where(enrollment => enrollment.CourseId ==  command.CourseId && enrollment.StudentId == userId).AnyAsync(cancellationToken);
        
        if(!isStudentEnrolled)
            return Result.Fail<ReviewDto>(ReviewErrors.InvalidReview);

        var reviewResult = Review.Create(Guid.NewGuid(), userId, command.CourseId, command.Rateing, command.Comment);
        if (reviewResult.IsFailure)
            return Result.Fail<ReviewDto>(reviewResult.Error);

        _context.Reviews.Add(reviewResult.Data!);
        await _context.SaveChangesAsync(cancellationToken);
        // here bug toDto not have student so null reference
        await _cache.RemoveByTagAsync("review");
        return Result.Success(reviewResult.Data.ToDto());
    }
}