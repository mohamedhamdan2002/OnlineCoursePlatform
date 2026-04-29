using Application.Features.Enrollments.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Enrollments.Queries.GetMyEnrollments;
public sealed record GetMyEnrollmentsQuery() : IRequest<Result<List<EnrollmentDto>>>;
