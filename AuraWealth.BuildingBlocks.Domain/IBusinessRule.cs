using System;
using System.Collections.Generic;
using System.Text;

namespace AuraWealth.BuildingBlocks.Domain
{
    public interface IBusinessRule
    {
        string Message { get; }
        bool IsBroken();
    }
}
