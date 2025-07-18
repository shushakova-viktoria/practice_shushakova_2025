using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

namespace task17
{
    public interface IScheduler
    {
        bool HasCommand();
        ICommand Select();
        void Add(ICommand cmd);
    }
    public interface InterfaceForLongCommand : ICommand
    {
       new bool Completed { get; }
    }

    public class Scheduler : IScheduler
    {
        public List<InterfaceForLongCommand> commands = new List<InterfaceForLongCommand>();
        int index = 0;

        public void Add(ICommand cmd)
        {
            if (cmd is InterfaceForLongCommand longCmd)
            {
                commands.Add(longCmd);
            }
            else
            {
                cmd.Execute();
            }
        }

        public bool HasCommand()
        {
            return commands.Count > 0;
        }

        public ICommand Select()
        {
            if (commands.Count == 0)
                return null;

            for (int i = 0; i < commands.Count; i++)
            {
                if (index >= commands.Count)
                {
                    index = 0;
                }

                var current = commands[index];

                if (current.Completed)
                {
                    commands.RemoveAt(index);
                    continue;
                }

                index = (index + 1) % commands.Count;
                return current;
            }

            return null;
        }

    }
    public interface ICommand
    {
        void Execute();
        bool Completed();
    }
    public class TestCommand : InterfaceForLongCommand 
    {
        public int Id { get; }
        public int Counter;
        public List<DateTime> Times { get; } = new List<DateTime>();
        public bool Completed => Counter >= 3;
        public TestCommand(int id) => Id = id;

        public void Execute()
        {
            Times.Add(DateTime.Now);
            Counter++;
            Console.WriteLine($"Команда {Id} выполняется (раз: {Counter})");
        }

        bool ICommand.Completed() => Completed;
    }

    public class ServerThread
    {
        public IScheduler Scheduler = new Scheduler();
        public ConcurrentQueue<ICommand> CommandQueue = new ConcurrentQueue<ICommand>();
        public bool hardStop = false;
        public bool softStop = false;
        public Thread thread;

        public ServerThread(IScheduler scheduler)
        {
            Scheduler = scheduler;
            thread = new Thread(Working);
        }

        public void Start()
        {
            thread.Start();
        }

        public void Adding(ICommand command)
        {
            Scheduler.Add(command);
        }

        public void HardStop()
        {
            hardStop = true;
        }

        public void SoftStop()
        {
            if (Thread.CurrentThread != thread)
            {
                throw new InvalidOperationException();
            }
            else
            {
                softStop = true;
            }

        }

        public void Working()
        {
            while (!hardStop)
            {
                var comm = Scheduler.Select();
                if (comm != null)
                {
                    try
                    {
                        comm.Execute();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return;
        }
    }

    public class HardStop : ICommand
    {
        public ServerThread _serverThread;

        public HardStop(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }
        public void Execute()
        {
            _serverThread.HardStop();
        }

        public bool Completed()
        {
            return true;
        }
    }

    public class SoftStop: ICommand
    {
        public ServerThread _serverThread;

        public SoftStop(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }
        public void Execute()
        {
            _serverThread.SoftStop();
        }

        public bool Completed()
        {
            return true;
        }
    }
}
