using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace StoreApp.Utils
{
    public class PropertiesUtil
    {
        public static List<string> GetClassProperties<T>()
        {
            List<string> properties = new List<string>();

            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                properties.Add(p.Name);
            }

            return properties;
        }
    }
}