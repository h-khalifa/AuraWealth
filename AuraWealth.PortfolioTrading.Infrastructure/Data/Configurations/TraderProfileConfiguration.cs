using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuraWealth.PortfolioTrading.Infrastructure.Data.Configurations
{
    public class TraderProfileConfiguration : IEntityTypeConfiguration<TraderProfile>
    {
        public void Configure(EntityTypeBuilder<TraderProfile> builder)
        {
            builder.ToTable("TraderProfiles");

            builder.HasKey(t => t.Id);

            // 1. Lock in the Value Object mapping to match your Dapper Query
            builder.OwnsOne(t => t.CashBalance, cb =>
            {
                cb.Property(p => p.Amount)
                  .HasColumnName("CashBalance") // <--- This guarantees Dapper won't break
                  .HasColumnType("decimal(18,4)") // Standard financial precision
                  .IsRequired();

                cb.Property(p => p.CurrencyCode)
                  .HasColumnName("Currency")
                  .HasMaxLength(3)
                  .IsRequired();
            });

            // 2. Map the AssetHoldings collection
            // Assuming AssetHolding is mapped as an Owned Entity or child Entity
            builder.OwnsMany(t => t.AssetHoldings, h =>
            {
                h.ToTable("AssetHoldings");
                h.Property(a => a.Quantity).HasColumnType("decimal(18,4)");

                // Force the Foreign Key name to match the Dapper query
                h.WithOwner().HasForeignKey("TraderProfileId");
            });

            // 3. CRITICAL: Tell EF Core to ignore the DomainEvents list!
            // Otherwise, it will try to create a database table called "DomainEvent"
            builder.Ignore(t => t.DomainEvents);
        }
    }
}
