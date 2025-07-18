namespace plotAndReport
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel.Design;
    using System.Threading;
    using System.Threading.Tasks;
    using task17;
    using ScottPlot;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    class Program
    {
        static void PlotPerformance(List<(int id, int calls, List<DateTime> times)> data)
        {
            var validData = data.Where(d => d.times != null && d.times.Count > 0).ToList();

            var plot = new ScottPlot.Plot();
            string path = @"C:\Users\vashu\practice_shushakova_2025\practice2025\PlotAndRep\plot.png";

            var allTimes = validData
                .SelectMany(d => d.times)
                .ToList();
            var startTime = allTimes
                .Min();

            foreach (var (id, calls, times) in validData)
            {
                double[] xValues = times.Select(t => (t - startTime).TotalMilliseconds).ToArray();
                double[] yValues = Enumerable.Repeat((double)id, times.Count).ToArray();

                var scatter = plot.Add.Scatter(xValues, yValues);
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
            thread.thread = new Thread(thread.Working);

            var testCommands = Enumerable.Range(1, 7)
            .Select(id => new TestCommand(id))
            .ToList();

            foreach (var cmd in testCommands)
            {
                scheduler.Add(cmd);
                Console.WriteLine($"Добавлена команда {cmd.Id}");
            }

            thread.Start();

            for (int i = 0; i < 3; i++) 
            {
                foreach (var cmd in testCommands)
                {
                    cmd.Execute();
                }
                Thread.Sleep(100); 
            }

            thread.HardStop();
            thread.thread.Join();

            var reportData = testCommands
            .Select(c => (c.Id, c.Counter, c.Times))
            .ToList();

            static void Report(List<(int id, int calls, List<DateTime> times)> reportData)
            {
                var validData = reportData.Where(d => d.times != null && d.times.Count > 0).ToList();

                if (!reportData.Any()) return;

                var allTimes = reportData
                    .SelectMany(s => s.times)
                    .OrderBy(t => t).ToList();

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
                string path = @"C:\Users\vashu\practice_shushakova_2025\practice2025\PlotAndRep\results.txt";
                File.WriteAllText(path, resultText);

            }

            Report(reportData);
            PlotPerformance(reportData);

        }
    }
}

