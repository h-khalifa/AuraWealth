namespace AuraWealth.BuildingBlocks.Application.Interfaces
{
    //used for commands, with ef
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
