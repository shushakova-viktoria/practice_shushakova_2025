using CommandLib;
using FileSystemCommands;
using System.IO;
using System.Reflection;
using static CommandLib.commandLib;

namespace CommandRunner
{
    class CommandLoader
    {
        static void Main(string[] args)
        {
            var dllFinder = Assembly.LoadFrom("FileSystemCommands.dll");

            foreach (Type type in dllFinder.GetTypes())
            {
                try
                {
                    var command = (ICommand)Activator.CreateInstance(type, new[] { "." });
                    command.Execute();
                }
                catch
                {
                    var command = (ICommand)Activator.CreateInstance(type, new[] { ".", "*.txt" });
                    command.Execute();
                }
            }

        }

    }
}
