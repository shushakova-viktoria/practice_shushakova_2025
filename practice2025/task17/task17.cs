using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace task17
{
    public interface ICommand
    {
        void Execute();
    }

    public class ServerThread
    {
        public ConcurrentQueue<ICommand> CommandQueue = new ConcurrentQueue<ICommand>();
        public bool hardStop = false;
        public bool softStop = false;
        public Thread thread;

        public void Initialization()
        {
            thread = new Thread(Working);
        }

        public void Start()
        {
            thread.Start();
        }

        public void ForQueue(ICommand command)
        {
            if (thread == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                CommandQueue.Enqueue(command);
            }
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
                if (CommandQueue.TryDequeue(out var command))
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    if (softStop)
                    {
                        break;
                    }
                }
            }
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
    }

}
