using System;
using System.Reflection;
using System.Collections.Generic;


namespace task05
{
    public class ClassAnalyzer
    {
        private Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods()
        {
            return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(s => s.Name);
        }

        public IEnumerable<string> GetMethodParams(string methodname)
        {
            return _type
               .GetMethod(methodname)?
               .GetParameters()
               .Select(s => s.Name!)??
            Enumerable.Empty<string>(); 
        }

        public IEnumerable<string> GetAllFields()
        {
           return _type
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Select (s => s.Name);
        }

        public IEnumerable<string> GetProperties()
        {
            return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(s => s.Name);
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return _type.GetCustomAttribute<T>() != null;
        }

    }


}
