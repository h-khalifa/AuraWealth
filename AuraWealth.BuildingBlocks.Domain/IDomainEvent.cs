using MediatR;

namespace AuraWealth.BuildingBlocks.Domain
{
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }
}
