using AuraWealth.MarketData.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuraWealth.MarketData.Infrastructure.Data.Configurations;

public sealed class PriceTickConfiguration : IEntityTypeConfiguration<PriceTick>
{
    public void Configure(EntityTypeBuilder<PriceTick> builder)
    {
        builder.ToTable("PriceTicks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Symbol)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(t => t.CurrencyCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.HasIndex(t => t.InstrumentId);
        builder.HasIndex(t => new { t.Symbol, t.ObservedAtUtc });
    }
}