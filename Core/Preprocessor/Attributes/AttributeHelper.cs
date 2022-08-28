using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyJobInfraCode.Core.Preprocessor.Attributes
{
    public static class AttributeHelper
    {
        public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>(
            Expression<Func<T, TOut>> propertyExpression,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attr != null ? valueSelector(attr) : default(TValue);
        }
        //var author = AttributeHelper.GetPropertyAttributeValue<Book, string, AuthorAttribute, string>(prop => prop.Name, attr => attr.Author);

        /*
        public static Dictionary<string, string> GetProperties(Type myObject)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();

            PropertyInfo[] props = typeof(myObject).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    AuthorAttribute authAttr = attr as AuthorAttribute;
                    if (authAttr != null)
                    {
                        string propName = prop.Name;
                        string auth = authAttr.Name;

                        _dict.Add(propName, auth);
                    }
                }
            }

            return _dict;
        }
        */
    }

}
