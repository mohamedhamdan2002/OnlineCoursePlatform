using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.Role)
            .HasConversion(
                value => value.ToString(),
                value => (AppRole) Enum.Parse(typeof(AppRole), value)
            );

        builder.HasMany(user => user.Enrollments)
            .WithOne()
            .HasForeignKey(enrollment => enrollment.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(user => user.Enrollments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
