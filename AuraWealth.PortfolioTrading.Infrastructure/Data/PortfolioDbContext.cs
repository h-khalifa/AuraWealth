using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuraWealth.PortfolioTrading.Infrastructure.Data
{
    public class PortfolioDbContext : DbContext, IUnitOfWork
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
        }
        // Define DbSets for your entities here
        public DbSet<PortfolioTrading.Domain.TraderProfiles.TraderProfile> TraderProfiles { get; set; }
        public DbSet<PortfolioTrading.Application.TraderProfiles.ReadModels.TransactionRecord> TransactionRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔥 THE MAGIC: All tables in this DbContext will default to this schema
            modelBuilder.HasDefaultSchema("portfolio");

            // Automatically load all our Fluent API configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortfolioDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        // IUnitOfWork implementation is handled natively by DbContext.SaveChangesAsync! 
    }
}
