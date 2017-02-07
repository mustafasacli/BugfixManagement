using System;
using System.Collections.Generic;

namespace BugfixManagement.Core.Difference
{
    internal interface IDifference
    {
        Type TypeOfObject { get; }

        Dictionary<string, object> Differences { get; }

        List<IDifferenceParam> DifferenceList { get; }
    }
}
