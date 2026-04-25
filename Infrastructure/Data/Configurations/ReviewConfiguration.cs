using Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(review => review.Id);
        builder.HasOne(review => review.Student)
            .WithMany()
            .HasForeignKey(review => review.StudentId);
        builder.HasOne(review => review.Course)
            .WithMany(course => course.Reviews)
            .HasForeignKey(review => review.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}
