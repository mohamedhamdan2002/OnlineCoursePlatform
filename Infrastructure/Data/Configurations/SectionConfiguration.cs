using Domain.Courses.Sections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(section => section.Id);

        builder.HasOne(section => section.Course)
            .WithMany(course => course.Sections)
            .HasForeignKey(section => section.CourseId).OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(section => section.Lectures)
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);
    }
}
