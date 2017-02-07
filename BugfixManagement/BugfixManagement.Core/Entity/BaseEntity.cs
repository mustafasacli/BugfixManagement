using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugfixManagement.Core.Entity
{
    public abstract class BaseEntity : IBaseEntity
    {
        public object Clone()
        {
            return (this.MemberwiseClone() as BaseEntity);
        }

        public virtual IBaseEntity Copy()
        {
            IBaseEntity be = this.Clone() as IBaseEntity;
            return be;
        }
    }
}
