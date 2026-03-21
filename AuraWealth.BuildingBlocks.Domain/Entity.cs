using System;
using System.Collections.Generic;
using System.Text;

namespace AuraWealth.BuildingBlocks.Domain
{
    public abstract class Entity
    {
        // 1. Identity
        /// <summary>
        /// The globally unique identifier for this entity. 
        /// 
        /// Architectural Decision: Guid is explicitly chosen over an auto-incrementing 'int' 
        /// to support a distributed architecture (Modular Monolith to Microservices). 
        /// 
        /// 1. Decentralized Generation: Allows the domain to generate the ID in-memory 
        ///    before database persistence, which is required for publishing complete Domain Events.
        /// 2. Global Uniqueness: Prevents ID collisions across different bounded contexts 
        ///    and isolated microservice databases.
        /// 3. Sharding/Merging: Ensures frictionless data migration without primary key conflicts.
        /// 
        /// Infrastructure Note: Ensure Entity Framework Core is configured to generate 
        /// Sequential Guids (e.g., ValueGeneratedOnAdd() / NEWSEQUENTIALID) to prevent 
        /// clustered index fragmentation.
        /// </summary>
        public Guid Id { get; protected set; }

        // 2. Domain Events List
        private List<IDomainEvent>? _domainEvents;

        // Expose as read-only so outside classes can't arbitrarily add events
        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        // 3. Equality Logic (Strictly by ID)
        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            // Transient entities (not saved to DB yet, so ID is empty) are never equal to each other
            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity? a, Entity? b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            // Multiply by a prime number to avoid hash collisions
            return (GetType().ToString() + Id).GetHashCode() * 97;
        }
    }
}
