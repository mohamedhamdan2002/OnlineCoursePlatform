using Domain.Categories;
using Domain.Courses;
using Domain.Courses.Lectures;
using Domain.Courses.Sections;
using Domain.Enrollments;
using Domain.Identity;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Category> Categories { get; }
    DbSet<Course> Courses { get; }
    DbSet<Section> Sections { get; }
    DbSet<Lecture> Lectures { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Enrollment> Enrollments { get; }



    ChangeTracker ChangeTracker { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
