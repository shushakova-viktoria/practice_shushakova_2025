using CommandLib;
using System;
using System.IO;
using static CommandLib.commandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        public string Outline;
        public long totalSize;

        public DirectorySizeCommand(string outline)
        {
            Outline = outline;
        }

        public void Execute()
        {
            totalSize = 0;
            foreach(var file in Directory.GetFiles(Outline))
            {
                totalSize += new FileInfo(file).Length;
            }
            
        }
    }

    public class FindFilesCommand: ICommand
    {
        public string Outline;
        public string SearchByMask;

        public FindFilesCommand(string outline, string searchByMask)
        {
            Outline = outline;
            SearchByMask = searchByMask;
        }

        public void Execute()
        {
            var file = Directory.GetFiles(Outline, SearchByMask);
        }
    }
}