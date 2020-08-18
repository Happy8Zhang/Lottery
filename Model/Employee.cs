///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/10/2020 10:53:44 AM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Demo.Lottery
{
    public class Employee
    {
        #region field
        string _basicInfo = null;
        readonly static Dictionary<string, string> _dicPropNames = null;
        #endregion

        #region property
        [Description("姓名")]
        public string Name { get; set; }
        [Description("部门")]
        public string Department { get; set; }
        [Description("工号")]
        public string Number { get; set; }
        [Description("电话")]
        public string Phone { get; set; }
        public string SimplePhone => string.IsNullOrEmpty(Phone) ? string.Empty : Phone.Replace(Phone.Substring(3, 4), "****");
        [Description("公司")]
        public string Company { get; set; }

        public string BasicInfo
        {
            get
            {
                _basicInfo ??= GetBasicInfo();
                return _basicInfo;
            }
        }
        #endregion

        #region constructor
        static Employee()
        {
            if (null != _dicPropNames)
            {
                return;
            }
            var props = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _dicPropNames = props.Select(prop => (prop.GetCustomAttributes(typeof(DescriptionAttribute), true)?.FirstOrDefault() is DescriptionAttribute attr) ?
                                                  new { Key = prop.Name, Value = attr.Description } : null).
                                  Where(item => null != item).
                                  ToDictionary(item => item.Key, item => item.Value);
        }
        #endregion

        #region method
        internal bool ContainBasicProperty(string item)
        {
            return item == Number || Name == item || Department == item || Company == item || Phone == item;
        }

        string GetBasicInfo()
        {
            var basicInfo = Name;
            basicInfo += AppendInfoWithSeparator(Department);
            basicInfo += AppendInfoWithSeparator(Number);
            basicInfo += AppendInfoWithNewLine(_dicPropNames[nameof(Phone)], SimplePhone);
            basicInfo += AppendInfoWithNewLine(_dicPropNames[nameof(Company)], Company);
            return basicInfo;
        }

        string AppendInfoWithSeparator(string info)
        {
            return string.IsNullOrEmpty(info) ? string.Empty : $"-{info}";
        }

        string AppendInfoWithNewLine(string displayName, string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : $"{Environment.NewLine}{displayName}: {value}";
        }

        bool GetPropertiesAndAttributes<TAttribute>(Type type, out IEnumerable<Tuple<PropertyInfo, TAttribute>> propsInfo) where TAttribute : Attribute
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
        #endregion
    }
}
