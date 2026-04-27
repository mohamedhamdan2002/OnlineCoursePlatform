using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Status)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(payment => payment.Provider)
            .HasConversion<string>()
            .HasMaxLength(10);
    }
}