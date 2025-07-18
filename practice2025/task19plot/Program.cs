using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using task17;
using ScottPlot;

class Program
{
    public static void PlotPerformance(List<(int id, int calls, List<DateTime> times)> data)
    {
        var plot = new ScottPlot.Plot();
        string path = @"C:\Users\vashu\practice_shushakova_2025\practice2025\plot.png";

        var allTimes = data
            .Where(s => s.times != null && s.times.Count > 0)
            .SelectMany(d => d.times)
            .ToList();

        var start = allTimes.Min();

        foreach (var (id, calls, times) in data)
        {
            if (times == null || times.Count == 0) continue;

            double[] xValues = times
                .Select(t => (t - start).TotalMilliseconds)
                .ToArray();

            double[] yValues = Enumerable.Repeat((double)id, times.Count).ToArray();

            var scatter = plot.Add.Scatter(xValues, yValues);
            scatter.LegendText = $"Команда {id}";
            scatter.MarkerSize = 8;
        }

        plot.XLabel("Время выполнения (мс)");
        plot.YLabel("ID команды");
        plot.Title("Выполнение длительных команд");

        plot.SavePng(path, 800, 600);
        Console.WriteLine($"График сохранён: {path}");
    }


    static void Main()
    {
        var scheduler = new Scheduler();
        var thread = new ServerThread(scheduler);
        var commands = new List<TestCommand>();

        for (int i = 1; i <= 7; i++)
        {
            var cmd = new TestCommand(i);
            commands.Add(cmd);
            scheduler.Add(cmd);
        }
        thread.Start();

        var copyList = new List<TestCommand>(commands);
        while (scheduler.HasCommand())
        {
            Thread.Sleep(100);
        }

        thread.HardStop();
        thread.thread.Join();

        var reportData = new List<(int id, int calls, List<DateTime> times)>();
        foreach (var cmd in copyList)
        {
            reportData.Add((cmd.Id, cmd.Counter, new List<DateTime>(cmd.Times)));
        }

        static void Report(List<(int id, int calls, List<DateTime> times)> reportData)
        {
            if (!reportData.Any()) return;

            var allTimes = reportData
                .SelectMany(s => s.times)
                .OrderBy(t => t);

            var startTime = allTimes.First();
            var endTime = allTimes.Last();
            var totalTime = (endTime - startTime).TotalMilliseconds;

            int totalCalls = reportData.Sum(s => s.calls);

            var fastest = reportData
                .Where(s => s.times.Count > 0)
                .OrderBy(s => (s.times.Last() - s.times.First()).TotalMilliseconds)
                .First();

            var slowest = reportData
                .Where(s => s.times.Count > 0)
                .OrderByDescending(s => (s.times.Last() - s.times.First()).TotalMilliseconds)
                .First();
            double averageTime = reportData
                .Where(s => s.times.Count > 0)
                .Average(s => (s.times.Last() - s.times.First()).TotalMilliseconds);

            string resultText = $@"

            Общее время выполнения команд: {totalTime:F2} мс
            Общее количество вызовов Execute: {totalCalls}
            Среднее время выполнения одной команды: {averageTime:F2} мс

            Самая быстрая команда: {fastest.id} (время: {(fastest.times.Last() - fastest.times.First()).TotalMilliseconds:F2} мс)
            Самая медленная команда: {slowest.id} (время: {(slowest.times.Last() - slowest.times.First()).TotalMilliseconds:F2} мс)
            ";
            string path = @"C:\Users\vashu\practice_shushakova_2025\practice2025\results.txt";
            File.WriteAllText(path, resultText);

        }

        Report(reportData);
        PlotPerformance(reportData);

    }
}
