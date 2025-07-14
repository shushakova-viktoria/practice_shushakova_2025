using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ScottPlot;

namespace ScottPlotInVisualStudio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Создаем элемент ScottPlot
            var formsPlot = new ScottPlot.FormsPlot();
            formsPlot.Dock = DockStyle.Fill;
            this.Controls.Add(formsPlot);

            // Путь к CSV
            string filePath = Path.Combine(Application.StartupPath, "performance.csv");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл performance.csv не найден!");
                return;
            }

            // Читаем CSV
            string[] lines = File.ReadAllLines(filePath);
            double[] threads = lines.Skip(1).Select(l => double.Parse(l.Split(',')[0])).ToArray();
            double[] times = lines.Skip(1).Select(l => double.Parse(l.Split(',')[1])).ToArray();

            // Настройка графика
            var plt = formsPlot.Plot;
            plt.Add.Scatter(threads, times);
            plt.Title("Производительность Solve() от числа потоков");
            plt.XLabel("Число потоков");
            plt.YLabel("Время выполнения (мс)");
            plt.Style(figureBackground: Color.White, dataBackground: Color.White);
            plt.AxisAuto();

            // Сохраняем график как PNG
            string pngPath = Path.Combine(Application.StartupPath, "performance_plot.png");
            plt.SaveFig(pngPath);

            // Обновляем график
            formsPlot.Refresh();

            MessageBox.Show($"График сохранён по пути:\n{pngPath}");
        }
    }
}
