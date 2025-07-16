using Xunit;
using task17;

namespace task18tests
{
    public class task18tests
    {
        public class ForLongCommand : InterfaceForLongCommand
        {
            int Steps;

            public ForLongCommand(int steps)
            {
                Steps = steps;
            }

            public bool Completed
            {
                get { return Steps <= 0; }
            }

            public void Execute()
            {
                if (Steps > 0)
                {
                    Steps = Steps - 1;
                }
            }
        }

        public class ForOneCommand : ICommand
        {
            private readonly Action _action;

            public ForOneCommand(Action action)
            {
                _action = action;
            }

            public void Execute()
            {
                _action();
            }
        }

        [Fact]
        public void TestForLongCommand()
        {
            var scheduler = new Scheduler();
            var command = new ForLongCommand(5);

            scheduler.Add(command);

            for (int i = 0; i < 5; i++)
            {
                var cmd = scheduler.Select();
                cmd.Execute();

                if (command.Completed)
                {
                    scheduler.commands.Remove(command);
                }
                    
            }

            Assert.False(scheduler.HasCommand());
        }

        [Fact]
        public void TestForOneCommand()
        {
            var scheduler = new Scheduler();
            bool executed; 

            var simple = new ForOneCommand(() => executed = true);

            scheduler.Add(simple);

            Assert.False(scheduler.HasCommand());
        }

        [Fact]
        public void TestIfThereAreALotOfCommands()
        {
            var scheduler = new Scheduler();
            var longCom1 = new ForLongCommand(1);
            var simple = new ForOneCommand(() => { });
            var longCom2 = new ForLongCommand(1);
            bool executed;

            var simple2 = new ForOneCommand(() => executed = true);

            scheduler.Add(longCom1);       
            scheduler.Add(simple2);     
            scheduler.Add(longCom2);       


            var cmd1 = scheduler.Select();
            cmd1.Execute();
            if (longCom1.Completed) scheduler.commands.Remove(longCom1);

            var cmd2 = scheduler.Select();
            cmd2.Execute();
            if (longCom2.Completed) scheduler.commands.Remove(longCom2);

            Assert.False(scheduler.HasCommand());
        }

        [Fact]
        public void TestIfNoCommands()
        {
            var scheduler = new Scheduler();

            var cmd = scheduler.Select();

            Assert.Null(cmd);
        }
    }
}
