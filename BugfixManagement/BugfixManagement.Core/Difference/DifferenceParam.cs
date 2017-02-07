using System;

namespace BugfixManagement.Core.Difference
{
    public class DifferenceParam : IDifferenceParam
    {
        public DifferenceParam(string paramName, Type t, object oldVal, object newVal)
        {
            this.Name = paramName;
            this.TypeOfParam = t;
            this.OldValue = oldVal;
            this.NewValue = newVal;
        }

        public string Name { get; private set; }

        public Type TypeOfParam { get; private set; }

        public object OldValue { get; private set; }

        public object NewValue { get; private set; }
    }
}
