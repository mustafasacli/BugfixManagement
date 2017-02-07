using System;

namespace BugfixManagement.Core.Difference
{
    public interface IDifferenceParam
    {
        string Name { get; }

        Type TypeOfParam { get; }

        object OldValue { get; }

        object NewValue { get; }
    }
}
