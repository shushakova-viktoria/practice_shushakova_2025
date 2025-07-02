using System;
using System.Reflection;


namespace task09
{
    class task09
    {
        static void Main(string[] args)
        {
            string dll = args[0];

            Assembly ass = Assembly.LoadFrom(dll);

            foreach(Type type in ass.GetTypes())
            {
                Console.WriteLine($"Имя класса: {type.Name}");

                foreach (var method in type.GetMethods())
                {
                    Console.WriteLine($"Метод класса: {method.Name}");
                    foreach (var par in method.GetParameters())
                    {
                        Console.WriteLine($"Параметр: {par.Name}, тип: {par.ParameterType.Name}");
                    }
                }

                foreach(var attribute in type.GetCustomAttributes())
                {
                    Console.WriteLine($"Аттрибут класса: {attribute.GetType().Name}");
                }

                foreach(var constructor in type.GetConstructors())
                {
                    Console.WriteLine($"Конструкторы класса: {constructor.Name}");
                    foreach(var par in constructor.GetParameters())
                    {
                        Console.WriteLine($"Параметр: {par.Name}, тип: {par.ParameterType.Name}");
                    }
                }
            }
        }
    }
}
