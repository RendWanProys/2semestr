using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private List<ProgressBar> progressBars = new List<ProgressBar>();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void NumberInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = NumberInputTextBox.Text;
            if (string.IsNullOrEmpty(inputText))
            {
                ProgressBarContainer.Children.Clear();
                progressBars.Clear(); 
                return;
            }
            if (!int.TryParse(inputText, out int number))
            {
                MessageBox.Show("Введите целое число.");
                return;
            }
            if (number < 1 || number > 10)
            {
                MessageBox.Show("Введите число от 1 до 10.");
                return;
            }
            CreateProgressBars(number);
        }
        private void CreateProgressBars(int count)
        {
            ProgressBarContainer.Children.Clear();
            progressBars.Clear(); 
            for (int i = 0; i < count; i++)
            {
                ProgressBar progressBar = new ProgressBar
                {
                    Height = 20,
                    Margin = new Thickness(0, 0, 0, 5),
                    Foreground = GetRandomBrush(),
                    Value = 0,
                    Maximum = 100
                };
                ProgressBarContainer.Children.Add(progressBar);
                progressBars.Add(progressBar);
            }
        }
        private async void FillProgressBarsButton_Click(object sender, RoutedEventArgs e)
        {
            if (progressBars.Count == 0)
            {
                MessageBox.Show("Сначала введите число и создайте прогресс-бары.");
                return;
            }
            List<Task> tasks = progressBars.Select(progressBar => FillProgressBarAsync(progressBar)).ToList();
            await Task.WhenAll(tasks);
        }
        private async Task FillProgressBarAsync(ProgressBar progressBar)
        {
            int totalFillTime = random.Next(1000, 20001);
            DateTime startTime = DateTime.Now;
            while (progressBar.Value < progressBar.Maximum && (DateTime.Now - startTime).TotalMilliseconds < totalFillTime)
            {
                double incrementAmount = random.NextDouble() * 2;
                if (progressBar.Value + incrementAmount > progressBar.Maximum)
                {
                    incrementAmount = progressBar.Maximum - progressBar.Value;
                }
                progressBar.Value += incrementAmount;
                int delayTime = random.Next(5, 26);
                await Task.Delay(delayTime);
                Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
            }
            progressBar.Value = progressBar.Maximum;
        }

        private Brush GetRandomBrush()
        {
            Color randomColor = Color.FromRgb(
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256));

            return new SolidColorBrush(randomColor);
        }
    }
}