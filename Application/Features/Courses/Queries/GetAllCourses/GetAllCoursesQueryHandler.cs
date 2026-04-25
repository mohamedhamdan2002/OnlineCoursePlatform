using Application.Common.Interfaces;
using Application.Common.Utilities;
using Application.Features.Categories.Dots;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Mappers;
using Domain.Categories;
using Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Features.Courses.Queries.GetAllCourses;

public sealed class GetAllCoursesQueryHandler(IAppDbContext context) : IRequestHandler<GetAllCoursesQuery, Result<PageList<CourseDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<PageList<CourseDto>>> Handle(GetAllCoursesQuery query, CancellationToken cancellationToken)
    {
        
        var coursesQuery = _context.Courses.AsNoTracking()
                                            .Include(course => course.Category)
                                            .Include(course => course.Instructor)
                                            .AsQueryable();

        if(query.CategoriesIds is not null)
        {
            coursesQuery = coursesQuery.Where(course => query.CategoriesIds.Values.Any(categoryId => categoryId == course.CategoryId));
        }

        var coursesCount = await coursesQuery.CountAsync(cancellationToken);

        var coursesDtos = await coursesQuery.Skip((query.PageNumber - 1) * query.PageSize)
                                            .Take(query.PageSize)
                                            .Select(course => course.ToDto())
                                            .ToListAsync(cancellationToken);

        // this query must to improve just select the columns you need only
        //  SELECT[c].[Id], [c].[AverageRating], [c].[CategoryId], [c].[Description], [c].[InstructorId], [c].[Level], [c].[Price], [c].[ReviewsCount], [c].[StudentsCount], [c].[ThumbnailUrl], [c].[Title], [c0].[Id], [c0].[Name], [a].[Id], [a].[AccessFailedCount], [a].[ConcurrencyStamp], [a].[Email], [a].[EmailConfirmed], [a].[FirstName], [a].[LastName], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[Role], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
        //FROM[Courses] AS[c]
        //INNER JOIN[Categories] AS[c0] ON[c].[CategoryId] = [c0].[Id]
        //INNER JOIN[AspNetUsers] AS[a] ON[c].[InstructorId] = [a].[Id]


        // after add pagination
        //  SELECT[t].[Id], [t].[AverageRating], [t].[CategoryId], [t].[Description], [t].[InstructorId], [t].[Level], [t].[Price], [t].[ReviewsCount], [t].[StudentsCount], [t].[ThumbnailUrl], [t].[Title], [c0].[Id], [c0].[Name], [a].[Id], [a].[AccessFailedCount], [a].[ConcurrencyStamp], [a].[Email], [a].[EmailConfirmed], [a].[FirstName], [a].[LastName], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[Role], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
        //FROM(
        //    SELECT[c].[Id], [c].[AverageRating], [c].[CategoryId], [c].[Description], [c].[InstructorId], [c].[Level], [c].[Price], [c].[ReviewsCount], [c].[StudentsCount], [c].[ThumbnailUrl], [c].[Title]
        //    FROM[Courses] AS[c]
        //    ORDER BY(SELECT 1)
        //    OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
        //) AS[t]
        //INNER JOIN[Categories] AS[c0] ON[t].[CategoryId] = [c0].[Id]
        //INNER JOIN[AspNetUsers] AS[a] ON[t].[InstructorId] = [a].[Id]

        // this after add the filters by categories ids
      //  SELECT[t].[Id], [t].[AverageRating], [t].[CategoryId], [t].[Description], [t].[InstructorId], [t].[Level], [t].[Price], [t].[ReviewsCount], [t].[StudentsCount], [t].[ThumbnailUrl], [t].[Title], [c0].[Id], [c0].[Name], [a].[Id], [a].[AccessFailedCount], [a].[ConcurrencyStamp], [a].[Email], [a].[EmailConfirmed], [a].[FirstName], [a].[LastName], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[Role], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
      //FROM(
      //    SELECT[c].[Id], [c].[AverageRating], [c].[CategoryId], [c].[Description], [c].[InstructorId], [c].[Level], [c].[Price], [c].[ReviewsCount], [c].[StudentsCount], [c].[ThumbnailUrl], [c].[Title]
      //    FROM[Courses] AS[c]
      //    WHERE[c].[CategoryId] IN(
      //        SELECT[q].[value]
      //        FROM OPENJSON(@__query_CategoriesIds_Values_0) WITH([value] uniqueidentifier '$') AS[q]
      //    )
      //    ORDER BY(SELECT 1)
      //    OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
      //) AS[t]
      //INNER JOIN[Categories] AS[c0] ON[t].[CategoryId] = [c0].[Id]
      //INNER JOIN[AspNetUsers] AS[a] ON[t].[InstructorId] = [a].[Id]

        return Result.Success(new PageList<CourseDto>
        {
            PageItems = coursesDtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = coursesCount,
            TotalPages = (int) Math.Ceiling(coursesCount / (double) query.PageSize)
        });
    }
}
