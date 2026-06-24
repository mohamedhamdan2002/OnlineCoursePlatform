using Domain.Enrollments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(enrollment => enrollment.Id);
        //builder.HasOne(enrollment => enrollment.Student)
        //    .WithMany()
        //    .HasForeignKey(enrollment => enrollment.StudentId);


        builder.HasOne(enrollment => enrollment.Course)
            .WithMany(course => course.Enrollments)
            .HasForeignKey(enrollment => enrollment.CourseId).OnDelete(DeleteBehavior.Restrict);

    }
}
