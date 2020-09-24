using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.ModelValidate
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BaseAttribute : Attribute
    {
        public virtual string error { get; set; }
        public abstract bool Validate(object value);
    }
}
