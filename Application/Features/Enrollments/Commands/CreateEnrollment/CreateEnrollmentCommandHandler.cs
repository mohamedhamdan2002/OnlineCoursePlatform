using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using Domain.Enrollments;
using Domain.Enrollments.Events;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Enrollments.Commands.CreateEnrollment;

public sealed class CreateEnrollmentCommandHandler(IAppDbContext context, UserManager<User> userManager) : IRequestHandler<CreateEnrollmentCommand, Result>
{
    private readonly IAppDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result> Handle(CreateEnrollmentCommand command, CancellationToken cancellationToken)
    {
        var isEnrollmentExist = await _context.Enrollments.Where(enrollment => enrollment.CourseId == command.CourseId && enrollment.StudentId == command.UserId).AnyAsync();
        if (isEnrollmentExist)
            return Result.Fail(ApplicationErrors.AlreadyEnrollmentExist);

        var isUserExist = await _userManager.Users.AnyAsync(user => user.Id == command.UserId);
        if (!isUserExist)
            return Result.Fail(ApplicationErrors.UserNotFound);

        var isCourseExist = await _context.Courses.AnyAsync(course => course.Id == command.CourseId);
        if (!isCourseExist)
            return Result.Fail(ApplicationErrors.CourseNotFound);

        var enrollmentResult = Enrollment.Create(Guid.NewGuid(), command.UserId, command.CourseId, command.PaymentId);
        if (enrollmentResult.IsFailure)
            return Result.Fail(enrollmentResult.Error);

        _context.Enrollments.Add(enrollmentResult.Data);
        enrollmentResult.Data.AddDomainEvent(new EnrollmentCreatedEvent
        {
            Course = enrollmentResult.Data.Course,
            EnrolledAt = enrollmentResult.Data.EnrolledAt,
            EnrollmentId = enrollmentResult.Data.Id,
            UserId = command.UserId,
        });
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
        
    }
}
