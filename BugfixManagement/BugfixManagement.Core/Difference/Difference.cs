using BugfixManagement.Core.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BugfixManagement.Core.Difference
{
    public class Difference<T> where T : IBaseEntity
    {
        public Difference(T t1, T t2)
        {
            if (t1 == null || t2 == null)
                throw new Exception("Objects can not ne null.");

            this.TypeOfObject = typeof(T);

            PropertyInfo[] propsArr = typeof(T).GetProperties();

            object valSrc, valDest;
            List<IDifferenceParam> lst = new List<IDifferenceParam>();
            foreach (PropertyInfo prpInf in propsArr)
            {
                valSrc = prpInf.GetValue(t1);
                valDest = prpInf.GetValue(t2);
                if (!object.Equals(valSrc, valDest))
                {
                    lst.Add(new DifferenceParam(prpInf.Name,
                        Nullable.GetUnderlyingType(prpInf.PropertyType) ?? prpInf.PropertyType, valSrc, valDest));
                }
            }

            this.DifferenceList = lst;
        }

        public Type TypeOfObject { get; private set; }

        public List<IDifferenceParam> DifferenceList { get; private set; }
    }
}
