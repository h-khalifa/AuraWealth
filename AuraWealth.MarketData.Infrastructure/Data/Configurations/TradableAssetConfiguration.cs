using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuraWealth.MarketData.Infrastructure.Data.Configurations;

public sealed class TradableAssetConfiguration : IEntityTypeConfiguration<TradableAsset>
{
    public void Configure(EntityTypeBuilder<TradableAsset> builder)
    {
        builder.ToTable("TradableAssets");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Symbol)
            .HasConversion(
                symbol => symbol.Value,
                value => new TickerSymbol(value))
            .HasColumnName("Symbol")
            .HasMaxLength(5)
            .IsRequired();

        builder.HasIndex(a => a.Symbol)
            .IsUnique();

        builder.Property(a => a.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.AssetClass)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Nested Owned Value Object: MarketQuote -> Money
        builder.OwnsOne(a => a.CurrentQuote, quote =>
        {
            quote.Property(q => q.ObservedAtUtc)
                .HasColumnName("Quote_ObservedAtUtc");

            quote.OwnsOne(q => q.Price, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Quote_Price")
                    .HasColumnType("decimal(18,4)");

                money.Property(m => m.CurrencyCode)
                    .HasColumnName("Quote_Currency")
                    .HasMaxLength(3);
            });
        });

        builder.Ignore(a => a.DomainEvents);
    }
}