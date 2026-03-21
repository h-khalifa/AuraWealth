using System;
using System.Collections.Generic;
using System.Text;

namespace AuraWealth.BuildingBlocks.Domain
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        // Depending on specific architectural flavors, some teams move the 
        // Domain Events list out of the base Entity class and put it exclusively 
        // here, because technically only Aggregate Roots should publish events.
        // Since we put them in Entity, this class serves mostly as a semantic boundary.
    }
}
