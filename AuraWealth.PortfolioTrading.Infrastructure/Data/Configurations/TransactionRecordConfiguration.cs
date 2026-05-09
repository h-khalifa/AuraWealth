using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuraWealth.PortfolioTrading.Infrastructure.Data.Configurations
{
    public class TransactionRecordConfiguration : IEntityTypeConfiguration<TransactionRecord>
    {
        public void Configure(EntityTypeBuilder<TransactionRecord> builder)
        {
            builder.ToTable("TransactionRecords");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Quantity).HasColumnType("decimal(18,4)");
            builder.Property(t => t.PricePerUnit).HasColumnType("decimal(18,4)");
            builder.Property(t => t.TotalAmount).HasColumnType("decimal(18,4)");

            // Add indexes for lightning-fast Dapper queries
            builder.HasIndex(t => t.TraderId);
            builder.HasIndex(t => t.OccurredOn);
        }
    }
}
