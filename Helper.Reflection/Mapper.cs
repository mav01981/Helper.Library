using System;
using System.Collections.Generic;
using System.Reflection;

namespace Helper.Reflection
{
    public static class Mapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICollection<string> DisplayStringCollection<T>(object obj) where T : new()
        {
            ICollection<string> propertyList = new List<string>();

            GetAllChildProperties<T>(obj, propertyList);

            return propertyList;
        }
        private static void GetAllChildProperties<T>(object obj, ICollection<string> result)
        {
            Type objectType = obj.GetType();

            IList<PropertyInfo> props = new List<PropertyInfo>(objectType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (!prop.PropertyType.IsValueType && prop.PropertyType != typeof(string))
                {
                    GetAllChildProperties<T>(prop.GetValue(obj), result);
                }
                else
                {
                    result.Add($"Name:{prop.Name}|Value:{prop.GetValue(obj)}");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T MapToDictionary<T>(IDictionary<string, object> dict) where T : new()
        {
            Type type = typeof(T);
            T result = (T)Activator.CreateInstance(type);
            foreach (var item in dict)
            {
                PropertyReflection.SetProperty(item.Key, result, item.Value);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T MapDictionaryToObject<T>(Dictionary<string, object> dict) where T : new()
        {
            T obj = new T();

            foreach (var item in dict)
            {
                PropertyReflection.SetProperty(item.Key, obj, item.Value);
            }

            return obj;
        }
    }
}
