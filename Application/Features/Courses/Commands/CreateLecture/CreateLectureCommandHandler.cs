using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Mappers;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Commands.CreateLecture;

public class CreateLectureCommandHandler(IAppDbContext context) : IRequestHandler<CreateLectureCommand, Result<LectureDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<LectureDto>> Handle(CreateLectureCommand command, CancellationToken cancellationToken)
    {
        var section = await _context.Sections
                            .Include(s => s.Lectures)
                            .FirstOrDefaultAsync(s => s.Id == command.SectionId, cancellationToken);

        if (section is null)
            return Result.Fail<LectureDto>(ApplicationErrors.SectionNotFound);

        var result = section.AddLecture(command.Title);

        if (result.IsFailure)
            return Result.Fail<LectureDto>(result.Error);

        _context.Lectures.Add(result.Data);
        // just for debug
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Data.ToDto());
    }
}
