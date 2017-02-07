using System;

namespace BugfixManagement.Common.Extensions
{
    public static class NumericExtesions
    {
        public static object GetValue<T>(this Nullable<T> t) where T : struct
        {
            object val = null;

            if (t.HasValue)
            {
                val = t.Value;
            }

            return val;
        }
    }
}
