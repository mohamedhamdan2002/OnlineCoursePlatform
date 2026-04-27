using Domain.Common.Results;
using MediatR;

namespace Application.Features.Enrollments.Commands.CreateEnrollment;

public sealed record CreateEnrollmentCommand(Guid UserId, Guid CourseId, Guid PaymentId) : IRequest<Result>;
