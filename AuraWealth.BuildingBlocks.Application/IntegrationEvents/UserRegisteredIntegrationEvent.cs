using MediatR;

namespace AuraWealth.BuildingBlocks.Application.IntegrationEvents
{
    public class UserRegisteredIntegrationEvent : INotification
    {
        public Guid UserId { get; init; }
    }
}
