using Application.Common.Interfaces;
using Application.Features.Categories.Mappers;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Mappers;
using Application.Features.Enrollments.Dtos;
using Domain.Enrollments.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Enrollments.EventHandlers;
public sealed class EnrollmentCreatedEventHandler(IServiceScopeFactory scopeFactory) : INotificationHandler<EnrollmentCreatedEvent>
{
    public async Task Handle(EnrollmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var enrollmentNotifier = scope.ServiceProvider.GetRequiredService<IEnrollmentNotifier>();
        var courseDto = await context.Courses.Where(course => course.Id == notification.CourseId).Include(course => course.Instructor).Include(course => course.Category).Select(course => new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            ImageUrl = course.ThumbnailUrl ?? string.Empty,
            Level = course.Level.ToString(),
            Instructor = $"{course.Instructor.FirstName} {course.Instructor.LastName}",
            Price = course.Price,
            IsEnrolled = true,
            Rating = course.AverageRating,
            ReviewsCount = course.ReviewsCount,
            StudentsCount = course.StudentsCount,
            Category = course.Category.ToDto(),
            Sections = course.Sections.ToListOfDto()

        }).FirstOrDefaultAsync();

        if (courseDto is null)
            return;

        await enrollmentNotifier.SendEnrollmentCreatedAsync(notification.UserId,
                new EnrollmentDto
                {
                    Course = courseDto,
                    EnrolledAt = notification.EnrolledAt,
                    Id = notification.EnrollmentId
                },
                cancellationToken
            );
    }
}
