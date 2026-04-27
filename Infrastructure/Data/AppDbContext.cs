using Application.Common.Interfaces;
using Domain.Categories;
using Domain.Common;
using Domain.Courses;
using Domain.Courses.Lectures;
using Domain.Courses.Sections;
using Domain.Enrollments;
using Domain.Identity;
using Domain.Payments;
using Domain.Reviews;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), IAppDbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Lecture> Lectures => Set<Lecture>();
    public DbSet<Review> Reviews => Set<Review>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync(cancellationToken);

        return await  base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity entity && entity.DomainEvents.Count != 0)
                .Select(e => (Entity)e.Entity)
                .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEvents.ForEach(async domainEvent =>
        {
            await publisher.Publish(domainEvent, cancellationToken);
        });

        domainEntities.ForEach(entity =>
        {
            entity.ClearDomainEvents();
        });
    }
}
