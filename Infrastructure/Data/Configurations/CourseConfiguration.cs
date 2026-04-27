using Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(course  => course.Id);
        builder.HasOne(course => course.Instructor)
            .WithMany()
            .HasForeignKey(course => course.InstructorId);
        
        builder.HasOne(course => course.Category)
            .WithMany()
            .HasForeignKey(course => course.CategoryId);

        builder.Property(course => course.Level)
            .HasMaxLength(15)
            .HasConversion<string>();

        builder.Navigation(course => course.Sections)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(course => course.Enrollments)
            .WithOne()
            .HasForeignKey(enrollment => enrollment.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
