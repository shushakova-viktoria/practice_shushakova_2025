using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using task17;

namespace task17tests
{
    public class task17tests
    {
        public class ForTests : ICommand
        {
            public Action _action;

            public ForTests(Action action)
            {
                _action = action;
            }

            public void Execute()
            {
                _action();
            }
        }

        [Fact]
        public void TestForStoppingHardStop()
        {
            var thread = new ServerThread();
            thread.Initialization();

            bool one = false;
            bool two = false;
            bool three = false;

            thread.ForQueue(new ForTests(() => one = true));
            thread.ForQueue(new HardStop(thread));
            thread.ForQueue(new ForTests(() => two = true));
            thread.ForQueue(new ForTests(() => three = true));

            thread.Start();
            Thread.Sleep(500);

            Assert.True(thread.hardStop);
            Assert.True(one);
            Assert.False(two);
            Assert.False(three);


        }
        [Fact]
        public void TestForStoppingSoftStop()
        {
            var thread = new ServerThread();
            thread.Initialization();

            bool one = false;
            bool two = false;
            bool three = false;

            thread.ForQueue(new ForTests(() => one = true));
            thread.ForQueue(new ForTests(() => two = true));
            thread.ForQueue(new ForTests(() => three = true));
            thread.ForQueue(new SoftStop(thread));


            thread.Start();
            Thread.Sleep(500);

            Assert.True(thread.softStop);
            Assert.True(one);
            Assert.True(two);
            Assert.True(three);

        }

    }
}
