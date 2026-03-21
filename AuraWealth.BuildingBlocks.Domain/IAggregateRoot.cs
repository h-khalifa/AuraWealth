using System;
using System.Collections.Generic;
using System.Text;

namespace AuraWealth.BuildingBlocks.Domain
{
    /// <summary>
    /// Marker interface to indicate that an entity is an Aggregate Root.
    /// Repositories should only be implemented for types that implement this interface.
    /// </summary>
    public interface IAggregateRoot
    {
    }
}
