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

namespace CheckFishWpf.Page
{
    /// <summary>
    /// Логика взаимодействия для CheckPage.xaml
    /// </summary>
    public partial class CheckPage : System.Windows.Controls.Page
    {
        public CheckPage()
        {
            InitializeComponent();
        }

        private void Otchet_Click(object sender, RoutedEventArgs e)
        {


            if (!int.TryParse(MinTemp.Text, out int minTemp) || !int.TryParse(MaxTemp.Text, out int maxTemp))
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для температур.");
                return;
            }

            string[] TempList = Temp.Text.Split(' ');
            List<int> temperatures = TempList.Select(t => int.TryParse(t.Trim(), out int temp) ? temp : 0).ToList();

            var violations = CheckTemp(temperatures, minTemp, maxTemp);
            result.Text = string.Join(Environment.NewLine, violations);
        }

        private List<string> CheckTemp(List<int> temperatures, int minTemp, int maxTemp)
        {
            List<string> checkMin = new List<string>();
            checkMin.Add(" ");
            List<string> checkMax = new List<string>();
            checkMax.Add(" ");
            List<string> checkGood = new List<string>
            {
                "Доставка произошла успешно"
            };

            int violationMin = 0;
            int violationMax = 0;
            DateTime startTime = Convert.ToDateTime(DateTimeStart.Text);

            for (int i = 0; i < temperatures.Count; i++)
            {
                if (temperatures[i] < minTemp)
                {
                    violationMin += 10;
                    checkMin.Add($"Время: {startTime.AddMinutes(i * 10):HH:mm} Факт: {temperatures[i]} Норма: {minTemp} Отклонение: {minTemp - temperatures[i]}");
                }
                else if (temperatures[i] > maxTemp)
                {
                    violationMax += 10;
                    checkMax.Add($"Время: {startTime.AddMinutes(i * 10):HH:mm} Факт: {temperatures[i]} Норма: {maxTemp} Отклонение: {temperatures[i] - maxTemp}");
                }
            }

            if (violationMin > Convert.ToInt32(TimeMinTemp.Text))
            {
                result.Text = "";
                checkMin[0] = $"Порог минимально допустимой температуры превышен на {violationMin - Convert.ToInt32(TimeMinTemp.Text)} минут.";
                return checkMin;
            }
            else if (violationMax > Convert.ToInt32(TimeMaxTemp.Text))
            {
                result.Text = "";
                checkMax[0] = $"Порог максимально допустимой температуры превышен на {violationMax - Convert.ToInt32(TimeMaxTemp.Text)} минут.";
                return checkMax;
            }
            else
            {
                return checkGood;
            }
        }
    }
}
