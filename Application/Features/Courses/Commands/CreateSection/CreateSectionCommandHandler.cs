using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Mappers;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Commands.CreateSection;

public sealed class CreateSectionCommandHandler(IAppDbContext context) : IRequestHandler<CreateSectionCommand, Result<SectionDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<SectionDto>> Handle(CreateSectionCommand command, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.Include(course => course.Sections).FirstOrDefaultAsync(course => course.Id == command.CourseId, cancellationToken);
        if(course is null)
        {
            return Result.Fail<SectionDto>(ApplicationErrors.CourseNotFound);
        }

        var result = course.AddSection(command.Title);
        if(result.IsFailure)
        {
            return Result.Fail<SectionDto>(result.Error);
        }
        _context.Sections.Add(result.Data);

        // just for debug
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
        }

        await _context.SaveChangesAsync(cancellationToken);
        var sectionDto = result.Data.ToDto();
        return Result.Success(sectionDto);
    }
}
