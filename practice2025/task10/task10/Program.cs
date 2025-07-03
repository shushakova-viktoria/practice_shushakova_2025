using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;

public interface IPlugin
{
    void Execute();
}

[AttributeUsage(AttributeTargets.All)]
public class PluginLoad : Attribute
{
    public string Name { get; set; }
    public string Dependence { get; set; }

    public PluginLoad(string name, string dependence)
    {
        Name = name;
        Dependence = dependence;
    }
}

public class FindingPluginLoader
{
    public static void PluginLoader(string PluginsInDirectory)
    {
        string[] files = Directory.GetFiles(PluginsInDirectory);
        Console.WriteLine($"Найденные файлы: {files.Length}");

        Assembly[] assemblies = new Assembly[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            assemblies[i] = Assembly.LoadFrom(files[i]);
        }

        int counterTypes = 0;
        foreach (Assembly assembly in assemblies)
        {
            counterTypes = counterTypes + assembly.GetTypes().Length;
        }
        Console.WriteLine($"Количество типов: {counterTypes}");

        Type[] types = new Type[counterTypes];
        int ind = 0;
        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                types[ind++] = type;
            }
        }

        Type[] plugins = new Type[50];
        string[] names = new string[50];
        string[] dependencies = new string[50];
        int[] completed = new int[50];
        int pluginCounting = 0;

        foreach (Type type in types)
        {
            PluginLoad attr = (PluginLoad)Attribute.GetCustomAttribute(type, typeof(PluginLoad));
            if (attr != null && pluginCounting < 50)
            {
                Console.WriteLine($"Плагин: {attr.Name}, зависимость: {attr.Dependence}");
                plugins[pluginCounting] = type;
                names[pluginCounting] = attr.Name;
                dependencies[pluginCounting] = attr.Dependence;
                pluginCounting++;
            }
        }
        Console.WriteLine($"Количество плагинов: {pluginCounting}");

        for (int trying = 0; trying < pluginCounting; trying++)
        {
            for (int i = 0; i < pluginCounting; i++)
            {
                if (completed[i] == 0)
                {
                    var canComplete = 1;
                    if (!string.IsNullOrEmpty(dependencies[i]))
                    {
                        canComplete = 0;
                        for (int j = 0; j < pluginCounting; j++)
                        {
                            if (names[j] == dependencies[i] && completed[j] == 1)
                            {
                                canComplete = 1;
                                break;
                            }
                        }
                    }
                    if (canComplete == 1)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(plugins[i]);
                        Console.WriteLine($"{names[i]}");
                        plugin.Execute();
                        completed[i] = 1;
                        Console.WriteLine($"Выполнено: {names[i]}");
                    }
                }

            }
        }
        for (int i = 0; i < pluginCounting; i++)
        {
            if (completed[i] == 0)
            {
                Console.WriteLine($"Пропущен: {names[i]} с зависимостью: {dependencies[i]})");
            }

        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        FindingPluginLoader.PluginLoader("Plugins");
    }
}