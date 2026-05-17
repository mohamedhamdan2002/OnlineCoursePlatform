using Application.Common.Interfaces;
using Application.Enrollments.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Enrollments.Queries.GetMyEnrollments;
public sealed record GetMyEnrollmentsQuery() : ICacheRequest<Result<List<EnrollmentDto>>>
{
    public string CacheKey => "user_enrollments";

    public string[] Tags => ["enrollment"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
