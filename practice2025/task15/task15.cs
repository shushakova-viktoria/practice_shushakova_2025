using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using task14;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static task14.DefiniteIntegral;
using ScottPlot;
using static SkiaSharp.HarfBuzz.SKShaper;
using ScottPlot.Colormaps;

namespace task15
{
    class Program
    {
        public static void PlotPerformance((int Threads, double Time)[] times)
        {
            var scottPlot = new ScottPlot.Plot();
            string path = @"C:\Users\vashu\practice_shushakova_2025\practice2025\task15\plot.png";

            scottPlot.XLabel("Время выполнения (мс)");
            double[] xValues = times.Select(s => s.Time).ToArray();

            scottPlot.YLabel("Количество потоков");
            double[] yValues = times.Select(r => Convert.ToDouble(r.Threads)).ToArray();

            scottPlot.Add.Scatter(xValues, yValues);

            scottPlot.Title("Производительность");
            scottPlot.SavePng(path, 800, 600);
        }

        static void Main()
        {

            double SinFunction(double x) => Math.Sin(x);
            double A = -100;
            double B = 100;
            double aim = 1e-4;
            int iterations = 3;
            int checkings = 10;

            double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };

            string resultFile = "results.txt";
            string csvFile = "csvResults.csv";

            StreamWriter writer = new StreamWriter(resultFile);
            StreamWriter csvWriter = new StreamWriter(csvFile);

            double betterStep = steps[0];
            double betterTime = double.MaxValue;

            foreach (var step in steps)
            {
                Stopwatch stopWatch = Stopwatch.StartNew();
                for (int i = 0; i < iterations; i++)
                {
                    SolveOneThread(A, B, SinFunction, step);
                }

                stopWatch.Stop();

                var averageTime = stopWatch.Elapsed.TotalMilliseconds / checkings;
                double result = SolveOneThread(A, B, SinFunction, step);

                if (Math.Abs(result) <= aim && averageTime < betterTime)
                {
                    betterStep = step;
                    betterTime = averageTime;
                }
            }

            int[] threadCounts = { 1, 2, 3, 4, 5 };
            var times = new List<(int Threads, double Time)>();


            foreach (int threads in threadCounts)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                for (int i = 0; i < checkings; i++)
                {
                    SolveManyThreads(A, B, SinFunction, betterStep, threads);
                }
                stopwatch.Stop();

                double averageTime = stopwatch.Elapsed.TotalMilliseconds / checkings;
                times.Add((threads, averageTime));
            }

            double oneTime = times.First(s => s.Threads == 1).Time;
            var bestManyThread = times.OrderBy(r => r.Time).First();

            var best = times.OrderBy(m => m.Time).First();

            double speedDifference = (oneTime - bestManyThread.Time) / oneTime * 100;

            string resultText = $@"
            Оптимальный шаг: {betterStep}
            Однопоточное время: {oneTime} мс
            Многопоточное время на {bestManyThread.Threads} потоков: {bestManyThread.Time} мс
            Лучше на: {speedDifference:F2}%
            ";

            string path = @"C:\Users\vashu\practice_shushakova_2025\practice2025\task15\results.txt";
            File.WriteAllText(path, resultText);

            PlotPerformance(times.ToArray());

        }
    }
}
