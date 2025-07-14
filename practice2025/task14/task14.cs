using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace task14
{
    public class DefiniteIntegral
    {
        public static double SolveManyThreads(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            double[] results = new double[threadsNumber];
            Thread[] threads = new Thread[threadsNumber];

            double countThreads = b - a;
            double sizeOfPart = (countThreads) / threadsNumber;

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
                });

                threads[i].Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            return Math.Round(results.Sum(), 2);
        }

        public static double SolveOneThread(double a, double b, Func<double, double> function, double step)
        {
            double end = 0.0;
            double x = a;

            while (x < b)
            {
                double nowX = Math.Min(x + step, b);
                end += (function(x) + function(nowX)) * (nowX - x) / 2;
                x = nowX;
            }

            return end;
        }


    }
}
