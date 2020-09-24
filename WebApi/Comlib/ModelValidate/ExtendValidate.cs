using Comlib.ModelValidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public static class AttributeExtend
{
    public static String Validate<T>(this T obj) where T : new()
    {
        Type type = obj.GetType();
        foreach (var item in type.GetProperties())
        {
            if (item.IsDefined(typeof(BaseAttribute), true))
            {
                BaseAttribute attribute = (BaseAttribute)item.GetCustomAttribute(typeof(BaseAttribute), true);
                if (!attribute.Validate(item.GetValue(obj)))
                {

                    throw new Exception(item.Name + ":" + attribute.error) { };

                }
            }
        }
        return "";
    }
}

