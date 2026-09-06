using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Service.Domain;

namespace Ordering.Service.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.ProductId)
            .IsRequired();
        
        builder.Property(o => o.Quantity)
            .IsRequired();
        
        builder.Property(o => o.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(o => o.CreatedAt)
            .IsRequired();
    }
}