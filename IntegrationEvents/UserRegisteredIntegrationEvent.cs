using MediatR;

namespace AuraWealth.BuildingBlocks.IntegrationEvents
{
    public class UserRegisteredIntegrationEvent : INotification
    {
        public Guid UserId { get; init; }
    }
}
