using Domain.Courses.Lectures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.HasKey(lecture => lecture.Id);
        builder.HasOne(lecture => lecture.Section)
            .WithMany(section => section.Lectures)
            .HasForeignKey(lecture => lecture.SectionId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(lecture => lecture.VideoStatus)
            .HasConversion<string>();
    }
}
