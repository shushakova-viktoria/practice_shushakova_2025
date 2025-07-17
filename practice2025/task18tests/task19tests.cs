using Xunit;
using task17;

namespace task18tests
{
    public class task19tests
    {
        [Fact]
        public void TestFor5TestCommands()
        {
            var scheduler = new Scheduler();
            var thread = new ServerThread(scheduler);
            int commands = 5;

            thread.Start();

            for (int i = 0; i < commands; i++)
            {
                scheduler.Add(new TestCommand(i));
            }
            int tryings = 0;

            while (scheduler.commands.Count > 0)
            {
                Thread.Sleep(100);
                tryings++;
            }

            thread.HardStop();
            thread.thread.Join();

            Assert.False(thread.thread.IsAlive);
        }
    }
}
