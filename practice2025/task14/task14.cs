using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace task14
{
    public class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            double[] results = new double[threadsNumber];
            Thread[] threads = new Thread[threadsNumber];

            Barrier barrier = new Barrier(threadsNumber + 1);

            double countThreads = b - a;
            double sizeOfPart = countThreads / threadsNumber;

            for (int i = 0; i < threadsNumber; i++)
            {
                int ind = i;

                threads[i] = new Thread(() =>
                {
                    var nowA = a + ind * sizeOfPart;

                    double nowB = 0;
            
                    if (ind == threadsNumber - 1)
                    {
                        nowB = b;
                    }

                    else
                    {
                        nowB = nowA + sizeOfPart;
                            
                    }
                    
                    double localSum = 0.0;
                    double x = nowA;

                    while (x < nowB)
                    {
                        double xMin = Math.Min(x + step, nowB);
                        localSum += (function(x) + function(xMin)) * (xMin - x) / 2;
                        x = xMin;
                    }

                    results[ind] = localSum;

                    barrier.SignalAndWait();
                });

                threads[i].Start();
            }

            barrier.SignalAndWait();

            double total = 0.0;
            foreach (var t in results)
            {
                total += t;
            }
            return Math.Round(total, 2);
        }
    }
}
