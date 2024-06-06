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
using TrackerLibrary;
using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts;
using LiveCharts.Wpf;

namespace Tracker
{
    /// <summary>
    /// Interaction logic for StatsUserControl.xaml
    /// </summary>
    public partial class StatsUserControl : UserControl
    {
        public User User { get; set; }
        public List<StudySession> StudySessions { get; set; }
        public List<StudySession> SessionsWeek { get; set; }
        public List<StudySession> SessionsMonth { get; set; }
        public List<StudySession> SessionsYear { get; set; }
        public int option;
        public ChartValues<double> SeriesCollection { get; set; }
        public StatsUserControl()
        {
            InitializeComponent();
        }

        public void LoadData() {

            DateTime today = DateTime.Today;
            int dayOfWeek = (int)today.DayOfWeek;
            DateTime startOfWeek = today.AddDays(-dayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7);
            SessionsWeek = User.StudySessions.Where(session => session.Date >= startOfWeek && session.Date < endOfWeek).OrderBy(session => session.Date).ToList();
            SessionsMonth = User.StudySessions.Where(u => u.Date.HasValue && u.Date.Value.Month == DateTime.Today.Month).ToList().OrderBy(session => session.Date).ToList();
            SessionsYear = User.StudySessions.Where(u => u.Date.HasValue && u.Date.Value.Year == DateTime.Today.Year).ToList().OrderBy(session => session.Date).ToList();
        }


        public void ShowGraph(int option) {
            if (option == 1)
            {
                var sessionsPerDay = new List<int>();
                DateTime today = DateTime.Today;
                int dayOfWeek = (int)today.DayOfWeek;
                DateTime monday = today.AddDays(-dayOfWeek + 1); //nech je zaciatok tyzdna pondelok a nie nedela
                int time = 0;
                int allCount = 0;

                for (int i = 0; i < 7; i++)
                {
                    DateTime day = monday.AddDays(i);
                    int countOfDay = 0;
                    foreach (var s in User.StudySessions) {
                        if (s.Date.HasValue && s.Date.Value.Date == day) {
                            countOfDay++;
                            time += s.Duration;
                            allCount++;
                        }
                    }
                    sessionsPerDay.Add(countOfDay);
                    LbCount.Content = "Count: " + allCount.ToString();
                    LbTime.Content = "Time studied: " + time.ToString() + " minutes";
                }


                CartesianChart ch = new()
                {
                    Series = new LiveCharts.SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Study sessions",
                        Values = new LiveCharts.ChartValues<int>(sessionsPerDay),
                        //Stroke = Brushes.LightBlue
                        Foreground = Brushes.White
                    }

                }
                };

                LiveCharts.Wpf.Axis axis = new()
                {
                    Labels = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" },
                    Foreground = Brushes.White,
                    FontSize = 15
                    
                };

                LiveCharts.Wpf.Axis axisY = new()
                {
                    Foreground = Brushes.White,
                    FontSize = 15,
                    Title = "Number of study sessions"
                };

                ch.AxisX.Add(axis);
                ch.AxisY.Add(axisY);
                ChartBorder.Child = ch;
                
            }
            else if (option == 2)
            {
                DateTime today = DateTime.Today;
                DateTime monthStart = new(today.Year, today.Month, 1);
                var sessionsPerDay = new List<int>();
                int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                int time = 0;
                int allCount = 0;

                for (int i = 0; i < daysInMonth; i++)
                {
                    DateTime day = monthStart.AddDays(i);
                    int countOfDay = 0;
                    foreach (var s in User.StudySessions)
                    {
                        if (s.Date.HasValue && s.Date.Value.Date == day)
                        {
                            countOfDay++;
                            time += s.Duration;
                            allCount++;
                        }
                    }
                    sessionsPerDay.Add(countOfDay);
                    LbCount.Content = "Count: " + allCount.ToString();
                    LbTime.Content = "Time studied: " + time.ToString() + " minutes";
                }


                CartesianChart ch = new()
                {
                    Series = new LiveCharts.SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Study sessions",
                        Values = new LiveCharts.ChartValues<int>(sessionsPerDay),
                        //Stroke = Brushes.LightBlue
                        Foreground = Brushes.White
                    }

                }
                };

                LiveCharts.Wpf.Axis axis = new()
                {
                    Labels = new[] { "Week 1", "Week 2", "Week 3", "Week 4" },
                    Foreground = Brushes.White,
                    FontSize = 15

                };

                LiveCharts.Wpf.Axis axisY = new()
                {
                    Foreground = Brushes.White,
                    FontSize = 15

                };

                ch.AxisX.Add(axis);
                ch.AxisY.Add(axisY);
                ChartBorder.Child = ch;

            }
            else if (option == 3)
            {
                DateTime today = DateTime.Today;
                var sessionsPerMonth = new List<int>();
                int time = 0;
                int allCount = 0;

                for (int month = 1; month <= 12; month++)
                {
                    DateTime startOfMonth = new(today.Year, month, 1);

                    int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                    int countOfDay = 0;
                    for (int i = 0; i < daysInMonth; i++)
                    {
                        DateTime day = startOfMonth.AddDays(i);
                        foreach (var s in User.StudySessions)
                        {
                            if (s.Date.HasValue && s.Date.Value.Date == day)
                            {
                                countOfDay++;
                                time += s.Duration;
                                allCount++;
                            }
                        }
                    }
                    sessionsPerMonth.Add(countOfDay);
                    LbCount.Content = "Count: " + allCount.ToString();
                    LbTime.Content = "Time studied: " + time.ToString() + " minutes";
                }

                CartesianChart ch = new()
                {
                    Series = new LiveCharts.SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Study sessions",
                        Values = new LiveCharts.ChartValues<int>(sessionsPerMonth),
                        //Stroke = Brushes.LightBlue
                        Foreground = Brushes.White
                    }

                }
                };

                LiveCharts.Wpf.Axis axis = new()
                {
                    Labels = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                    Foreground = Brushes.White,
                    FontSize = 15

                };

                LiveCharts.Wpf.Axis axisY = new()
                {
                    Foreground = Brushes.White,
                    FontSize = 15

                };

                ch.AxisX.Add(axis);
                ch.AxisY.Add(axisY);
                ChartBorder.Child = ch;

            }
            else if (option == 0) {
                
            }
             
        }

        private void RbWeek_Click(object sender, RoutedEventArgs e)
        {
            option = 1;
            ShowGraph(1);
        }

        private void RbMonth_Click(object sender, RoutedEventArgs e)
        {
            option = 2;
            ShowGraph(2);
        }

        private void RbYear_Click(object sender, RoutedEventArgs e)
        {
            option = 3;
            ShowGraph(3);
        }
    }
}
