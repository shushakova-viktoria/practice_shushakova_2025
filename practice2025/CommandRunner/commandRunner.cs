using CommandLib;
using FileSystemCommands;
using System.IO;
using System.Reflection;
using static CommandLib.commandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentCmd = new DirectorySizeCommand(".");
            currentCmd.Execute();
            Console.WriteLine($"Текущий каталог имеет размер: {currentCmd.totalSize}");

            var findTxt = new FindFilesCommand(".", "*.txt");
            findTxt.Execute();
            Console.WriteLine("Найдены файлы:");
            foreach (var file in Directory.GetFiles(findTxt.Outline, findTxt.SearchByMask))
            {
                Console.WriteLine($"Найден файл: {file}");
            }
        }
    }
    
}
