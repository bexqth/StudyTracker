using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Xml.Serialization;
using TrackerLibrary;
using static System.Collections.Specialized.BitVector32;
using Color = System.Drawing.Color;

namespace Tracker
{
    /// <summary>
    /// Interaction logic for TimerWindow.xaml
    /// </summary>
    public partial class TimerWindow : UserControl
    {
        private System.Windows.Media.Color LightBlueColor;
        private System.Windows.Media.Color DarkBlueColor;
        private System.Windows.Media.Color LightRedColor;
        private System.Windows.Media.Color DarkRedColor;
        private System.Windows.Media.Color LightGreenColor;
        private System.Windows.Media.Color DarkGreenColor;
        private TimeSpan PomodoroDuration;
        private TimeSpan ShortBreakDuration;
        private TimeSpan LongBreakDuration;
        private readonly System.Windows.Threading.DispatcherTimer DispatcherTimer;
        private bool PomodoroFocused;
        private bool ShortBreakFocused;
        private bool LongBreakFocused;

        private bool PomodoroStart;
        private bool ShortBreakStart;
        private bool LongBreakStart;

        private readonly int PomodoroDurationTime = 1;
        private readonly int ShortBreakDurationTime = 5;
        private readonly int LongBreakDurationTime = 15;
        private bool IsPaused = false;

        private int TotalSeconds;
        private double PassedSeconds;

        public User User { get; set; }

        public TimerWindow()
        {
            InitializeComponent();
            LightBlueColor = System.Windows.Media.Color.FromArgb(255, 181, 213, 245);
            LightRedColor = System.Windows.Media.Color.FromArgb(255, 217, 136, 128);
            LightGreenColor = System.Windows.Media.Color.FromArgb(255, 169, 223, 191);

            DarkBlueColor = System.Windows.Media.Color.FromArgb(255, 152, 193, 234);
            DarkRedColor = System.Windows.Media.Color.FromArgb(255, 205, 97, 85);
            DarkGreenColor = System.Windows.Media.Color.FromArgb(255, 125, 206, 160);

            DispatcherTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            DispatcherTimer.Tick += Timer_Tick;

            this.PomodoroDuration = TimeSpan.FromMinutes(PomodoroDurationTime);
            this.ShortBreakDuration = TimeSpan.FromMinutes(ShortBreakDurationTime);
            this.LongBreakDuration = TimeSpan.FromMinutes(LongBreakDurationTime);
            this.PomodoroFocused = true;
            PomodoroStart = false;
            ShortBreakStart = false;
            LongBreakStart = false;
            ChangeColor("blue");
        }

        public void ResetTime() {
            this.PomodoroDuration = TimeSpan.FromMinutes(PomodoroDurationTime);
            this.ShortBreakDuration = TimeSpan.FromMinutes(ShortBreakDurationTime);
            this.LongBreakDuration = TimeSpan.FromMinutes(LongBreakDurationTime);
            if (PomodoroFocused) {
                TbTime.Text = PomodoroDuration.ToString(@"hh\:mm\:ss");
            } else if (ShortBreakFocused) {
                TbTime.Text = ShortBreakDuration.ToString(@"hh\:mm\:ss");
            } else if (LongBreakFocused) {
                TbTime.Text = LongBreakDuration.ToString(@"hh\:mm\:ss");
            }
            this.IsPaused = false;
            BtStart.Content = "Start";
            DispatcherTimer.Stop();
            this.PassedSeconds = 0;
            PbTime.Value = 0;
            this.PassedSeconds = 0;
        }

        public void ChangeColor(string color) {
            switch (color) {
                case "blue":
                    TbTime.Text = PomodoroDuration.ToString();
                    InsideBorder.Background = new SolidColorBrush(LightBlueColor);
                    PbTime.Background = new SolidColorBrush(LightBlueColor);
                    BtPomodoro.Background = new SolidColorBrush(LightBlueColor);
                    BtLongBreak.Background = new SolidColorBrush(LightBlueColor);
                    BtShortBreak.Background = new SolidColorBrush(LightBlueColor);
                    BtStart.Background = new SolidColorBrush(DarkBlueColor);
                    break;

                case "red":
                    TbTime.Text = ShortBreakDuration.ToString();
                    InsideBorder.Background = new SolidColorBrush(LightRedColor);
                    PbTime.Background = new SolidColorBrush(LightRedColor);
                    BtPomodoro.Background = new SolidColorBrush(LightRedColor);
                    BtLongBreak.Background = new SolidColorBrush(LightRedColor);
                    BtShortBreak.Background = new SolidColorBrush(LightRedColor);
                    BtStart.Background = new SolidColorBrush(DarkRedColor);
                    break;

                case "green":
                    TbTime.Text = LongBreakDuration.ToString();
                    PbTime.Background = new SolidColorBrush(LightGreenColor);
                    InsideBorder.Background = new SolidColorBrush(LightGreenColor);
                    BtPomodoro.Background = new SolidColorBrush(LightGreenColor);
                    BtLongBreak.Background = new SolidColorBrush(LightGreenColor);
                    BtShortBreak.Background = new SolidColorBrush(LightGreenColor);
                    BtStart.Background = new SolidColorBrush(DarkGreenColor);
                    break;

            }
            
        }

        private void BtPomodoro_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor("blue");
            PomodoroFocused = true;
            ShortBreakFocused = false;
            LongBreakFocused = false;
            ResetTime();
        }

        private void BtShortBreak_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor("red");
            PomodoroFocused = false;
            ShortBreakFocused = true;
            LongBreakFocused = false;
            ResetTime();

        }

        private void BtLongBreak_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor("green");
            PomodoroFocused = false;
            ShortBreakFocused = false;
            LongBreakFocused = true;
            ResetTime();
        }

        private void BtStart_Click(object sender, RoutedEventArgs e)
        {
            if (IsPaused == false){
                DispatcherTimer.Start();
                BtStart.Content = "Stop";
                IsPaused = true;
            }
            else{
                DispatcherTimer.Stop();
                BtStart.Content = "Start";
                IsPaused = false;
            }

            if (PomodoroFocused && IsPaused)
            {
                PomodoroStart = true;
                this.TotalSeconds = PomodoroDurationTime * 60;
            }
            else if (ShortBreakFocused && IsPaused)
            {
                ShortBreakStart = true;
                this.TotalSeconds = ShortBreakDurationTime * 60;
            }
            else if (LongBreakFocused && IsPaused)
            {
                LongBreakStart = true;
                this.TotalSeconds = LongBreakDurationTime * 60;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (PomodoroFocused && PomodoroStart) {
                PomodoroDuration -= TimeSpan.FromMilliseconds(100);

                this.PassedSeconds += 0.1;

                TbTime.Text = PomodoroDuration.ToString(@"hh\:mm\:ss");
                PbTime.Value = ((double)this.PassedSeconds / this.TotalSeconds) * 100;
                if (PomodoroDuration == TimeSpan.Zero) {
                    PomodoroStart = false;
                    this.ResetTime();
                    User.AddStudySession(new StudySession(PomodoroDuration.Minutes,DateTime.Today));
                    User.SaveInCSV();
                }
            }
            else if (ShortBreakFocused && ShortBreakStart)
            {
                ShortBreakDuration -= TimeSpan.FromMilliseconds(100);
                this.PassedSeconds += 0.1;
                TbTime.Text = ShortBreakDuration.ToString(@"hh\:mm\:ss");
                PbTime.Value = ((double)this.PassedSeconds / this.TotalSeconds) * 100;
                if (PomodoroDuration == TimeSpan.Zero)
                {
                    this.ResetTime();
                    ShortBreakStart = false;
                }
                
            }
            else if (LongBreakFocused && LongBreakStart)
            {
                LongBreakDuration -= TimeSpan.FromMilliseconds(100);
                this.PassedSeconds += 0.1;
                TbTime.Text = LongBreakDuration.ToString(@"hh\:mm\:ss");
                PbTime.Value = ((double)this.PassedSeconds / this.TotalSeconds) * 100;
                if (PomodoroDuration == TimeSpan.Zero)
                {
                    this.ResetTime();
                    LongBreakStart = false;
                }
                
            }
        }

    }
}
