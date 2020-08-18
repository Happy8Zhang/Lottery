using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public static class TypeExtension
{
    public static bool GetPropertiesAndAttributes<TAttribute>(this Type type, out IEnumerable<Tuple<PropertyInfo, TAttribute>> propsInfo) where TAttribute : Attribute
    {
        propsInfo = null;
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (null == props && 0 == props.Length)
        {
            return false;
        }

        propsInfo = props.Select(prop =>
        {
            var attr = prop.GetCustomAttributes(typeof(TAttribute), true)?.FirstOrDefault() as TAttribute;
            return new Tuple<PropertyInfo, TAttribute>(prop, attr);
        }).Where(item => null != item.Item2);
        return null != propsInfo && propsInfo.Any();
    }

    /// <summary>
    /// 相同属性名称的属性值拷贝
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU"></typeparam>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    public static void CopyPropertiesValueWithPropName<T, TU>(this T source, TU dest)
    {
        var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
        var destProps = typeof(TU).GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

        foreach (var sourceProp in sourceProps)
        {
            if (destProps.Any(x => x.Name == sourceProp.Name))
            {
                var p = destProps.First(x => x.Name == sourceProp.Name);
                if (p.CanWrite)
                { // check if the property can be set or no.
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }
            }
        }
    }
}
