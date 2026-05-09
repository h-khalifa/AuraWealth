using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using AuraWealth.PortfolioTrading.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuraWealth.PortfolioTrading.Infrastructure.Repositories
{
    public class TraderProfileRepository : IDomainRepository<TraderProfile>
    {
        private readonly PortfolioDbContext _dbContext;

        public TraderProfileRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TraderProfile item, CancellationToken cancellationToken = default)
        {
            await _dbContext.TraderProfiles.AddAsync(item, cancellationToken);
        }

        public async Task DeleteAsync(TraderProfile item, CancellationToken cancellationToken = default)
        {
            _dbContext.TraderProfiles.Remove(item);
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TraderProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TraderProfiles
            .Include(t => t.AssetHoldings)//future proof against changing the configuration from OwnsMany to HasMany
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
    }
}
