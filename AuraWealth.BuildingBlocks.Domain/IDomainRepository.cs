namespace AuraWealth.BuildingBlocks.Domain
{
    public interface IDomainRepository<TEntity> where TEntity : IAggregateRoot
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity item, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity item, CancellationToken cancellationToken = default);
    }
}
