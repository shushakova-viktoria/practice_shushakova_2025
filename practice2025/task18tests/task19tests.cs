using Xunit;
using task17;

namespace task19tests
{
    public class task19tests
    {
        [Fact]
        public void TestFor5TestCommands()
        {
            var scheduler = new Scheduler();
            var thread = new ServerThread(scheduler);
            int commands = 5;

            for (int i = 0; i < commands; i++)
            {
                scheduler.Add(new TestCommand(i));
            }

            thread.Start();

            int maxDoings = 100;
            int doings = 0;
            while (scheduler.commands.Count > 0 && doings++ < maxDoings)
            {
                Thread.Sleep(100);
            }

            thread.HardStop();
            thread.thread.Join();

            Assert.False(thread.thread.IsAlive);

            foreach (var cmd in scheduler.commands.OfType<TestCommand>())
            {
                Assert.True(cmd.Completed);
            }
        }
    }
}
