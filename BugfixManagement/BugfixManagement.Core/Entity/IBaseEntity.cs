using System;

namespace BugfixManagement.Core.Entity
{
    public interface IBaseEntity : ICloneable
    {
        IBaseEntity Copy();
    }
}
