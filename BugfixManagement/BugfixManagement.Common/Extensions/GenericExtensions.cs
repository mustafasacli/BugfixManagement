using System;

namespace BugfixManagement.Common.Extensions
{
    public static class GenericExtensions
    {
        public static T AsType<T>(this object obj, bool allowNullable = false) where T : class
        {
            T rT = obj as T;

            if (!allowNullable)
            {
                if (rT == null)
                {
                    rT = Activator.CreateInstance<T>();
                }
            }

            return rT;
        }
    }
}
