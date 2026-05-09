using AuraWealth.BuildingBlocks.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AuraWealth.PortfolioTrading.Infrastructure.Interceptors
{
    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IPublisher _publisher;
        public DispatchDomainEventsInterceptor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

            // 1. Find all entities that have events
            var entities = dbContext.ChangeTracker
                .Entries<Entity>()//domain entity
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            // 2. Extract the events
            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            // 3. Clear the events from the entities (so they don't fire twice)
            entities.ForEach(e => e.ClearDomainEvents());

            // 4. Publish to MediatR (This triggers your AssetBoughtHandler!)
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
