using System;
using System.Reflection;
using static task07.task07;

namespace task07
{
    public class task07
    {
        [AttributeUsage(AttributeTargets.All)]
        public class DisplayNameAttribute : Attribute
        {
            public string DisplayName { get; set; }
            public DisplayNameAttribute(string displayName)
            {
                DisplayName = displayName;
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        public class VersionAttribute : Attribute
        {
            public int Major { get; set; }
            public int Minor { get; set; }
            public VersionAttribute(int major, int minor)
            {
                Major = major;
                Minor = minor;
            }
        }

        [DisplayName("Пример класса")]
        [Version(1, 0)]
        public class SampleClass
        {
            [DisplayName("Числовое свойство")]
            [Version(1, 0)]
            public int Number {  get; set; }

            [DisplayName("Тестовый метод")]
            public void TestMethod() { }
            
        }

    }

    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var className = type.GetCustomAttribute<DisplayNameAttribute>();
            if (className != null)
            {
                Console.WriteLine($"Имя класса: {className.DisplayName}");
            }

            var classVersion = type.GetCustomAttribute<VersionAttribute>();
            if (classVersion != null)
            {
                Console.WriteLine($"Версия класса: {classVersion.Major}.{classVersion.Minor}");
            }

            foreach (var method in type.GetMethods())
            {
                var listMethods = method.GetCustomAttribute<DisplayNameAttribute>();
                if (listMethods != null)
                {
                    Console.WriteLine($"Cписок методов с их DisplayName - {method.Name} : {listMethods.DisplayName}");
                }
            }

            foreach (var properties in type.GetProperties())
            {
                var listProperties = properties.GetCustomAttribute<DisplayNameAttribute>();
                if (listProperties != null)
                {
                    Console.WriteLine($"Список свойств с их DisplayName - {properties.Name} : {listProperties.DisplayName}");
                }
            }
        }
    }
}
