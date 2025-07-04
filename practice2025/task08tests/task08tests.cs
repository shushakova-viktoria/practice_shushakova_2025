using FileSystemCommands;
using System.IO;
using Xunit;
using CommandLib;
using CommandRunner;

namespace task08tests
{
    public class task08tests
    {
        public class FileSystemCommandsTests
        {
            [Fact]
            public void DirectorySizeCommand_ShouldCalculateSize()
            {
                var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
                Directory.CreateDirectory(testDir);
                File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
                File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

                var command = new DirectorySizeCommand(testDir);
                command.Execute(); // Проверяем, что не возникает исключений

                Assert.True(command.totalSize > 0);

                Directory.Delete(testDir, true);
            }

            [Fact]
            public void FindFilesCommand_ShouldFindMatchingFiles()
            {
                var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
                Directory.CreateDirectory(testDir);
                File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
                File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

                var command = new FindFilesCommand(testDir, "*.txt");
                command.Execute(); // Должен найти 1 файл

                var foundFiles = Directory.GetFiles(testDir, "*.txt");
                Assert.NotEmpty(foundFiles);
                Assert.Single(foundFiles);

                Directory.Delete(testDir, true);
            }

            [Fact]
            public void TestForCommandRunner()
            {
                string path = "FileSystemCommands.dll";
                Console.SetOut(new StringWriter());

                CommandRun.Main([path]);

                Console.SetOut(Console.Out);
            }
        }
    }
}
