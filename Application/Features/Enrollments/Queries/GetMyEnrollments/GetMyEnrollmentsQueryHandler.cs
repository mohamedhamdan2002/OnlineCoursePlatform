using Application.Common.Interfaces;
using Application.Features.Enrollments.Dtos;
using Application.Features.Enrollments.Mappers;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Enrollments.Queries.GetMyEnrollments;

public sealed class GetMyEnrollmentsQueryHandler(IAppDbContext context, ICurrentUser currentUser) : IRequestHandler<GetMyEnrollmentsQuery, Result<List<EnrollmentDto>>>
{
    private readonly IAppDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    async Task<Result<List<EnrollmentDto>>> IRequestHandler<GetMyEnrollmentsQuery, Result<List<EnrollmentDto>>>.Handle(GetMyEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollmentDtos = await _context.Enrollments.AsNoTracking()
            .Where(enrollment => enrollment.StudentId == _currentUser.UserId)
            .Include(enrollment => enrollment.Course)
                .ThenInclude(course => course.Instructor)
            .Include(enrollment => enrollment.Course)
                .ThenInclude(course => course.Category)
            .Select(enrollment => enrollment.ToDto())
            .ToListAsync();

        return Result.Success(enrollmentDtos);
    }
}
