using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Helper.Reflection
{
    public static class PropertyReflection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property != null)
            {
                var displayNameAttribute = property.GetCustomAttribute(typeof(DisplayNameAttribute), false) as DisplayNameAttribute;

                if (displayNameAttribute != null)
                {
                    return displayNameAttribute.DisplayName;
                }
            }

            return String.Join(".", memberExpression.ToString().Split('.').Skip(1).Select(p => p.ToString()).ToArray());
        }
        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(T obj, Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property != null)
            {
                return GetPropertyValue(obj, String.Join(".",
                    memberExpression.ToString().Split('.').Skip(1).Select(p => p.ToString()).ToArray()));
            }

            return null;
        }
        public static object SetPropertyValue<T>(T obj, string propertyName, T value)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            var matchedProperty = properties.FirstOrDefault(x => x.Name == propertyName);

            if (matchedProperty != null)
            {
                matchedProperty.SetValue(obj, value);
            }

            return obj;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);

                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);

                return prop != null ? prop.GetValue(src, null) : null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compoundProperty"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetProperty(string compoundProperty, object target, object value)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
                target = propertyToGet.GetValue(target, null);
            }
            
            PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());

            if (propertyToSet.PropertyType == typeof(int))
            {
                value = Convert.ToInt32(value);
            }

            propertyToSet.SetValue(target, value, null);
        }
    }
}