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
using Syncfusion.UI.Xaml.ProgressBar;
using Task = TrackerLibrary.Task;
using System.Collections.ObjectModel;
using LiveCharts.Wpf;
using LiveCharts;

namespace Tracker
{
    /// <summary>
    /// Interaction logic for ClassesUserControl.xaml
    /// </summary>
    public partial class ClassesUserControl : UserControl
    {
        public User User { get; set; }
        public Class Class { get; set; }
        private int focusedListBox;
        private bool gradeFormularShown;

        public ClassesUserControl()
        {
            InitializeComponent();
            gradeFormularShown = false;
        }


        public delegate void DisplayEditFormularEventHandler(List<ClassTask> focusedList, ListBox focusedListBox); //DELEGAT
        public event DisplayEditFormularEventHandler DisplayEditFormular; //UDALOST

        public void EditSelectedItem(List<ClassTask> focusedList, ListBox focusedListBox)
        {
            DisplayEditFormular?.Invoke(focusedList, focusedListBox);
        }

        public delegate void DisplayNewFormularEventHandler(); //DELEGAT
        public event DisplayNewFormularEventHandler DisplayNewFormular; //UDALOST

        public void DisplayClasses()
        {
            int a = 0;
            for (int i = 0; i < spButtons.Children.Count; i++)
            {
                if (spButtons.Children[i] is Button button) {
                    if (a < User.Classes.Count) { 
                        button.Content = User.Classes[a].ClassName;
                        a++;
                    }
                }
            }

            this.Class = User.Classes[0];
            FillBoxes();
            ChangeName();
        }

        public delegate void DisplayGradesEventHandler(); //DELEGAT
        public event DisplayGradesEventHandler DisplayGrades; //UDALOST


        public void ChangeName() {
            LClassName.Content = Class.ClassName;
        }

        public void DisplayGradePoints() {
            Class.UpdatePoints();
            LbRecievedPoints.Content = "Recieved points - " + Class.RecievedPoints.ToString();
            LbMaxPoints.Content = "Max points - " + Class.AllPoints.ToString();
            if (Class.AllPoints == 0) {
                LbPercentage.Content = "Percentage - " + 0 + " %";
            }
            else {
                double percentage = (Class.RecievedPoints / Class.AllPoints) * 100;
                percentage = Math.Round(percentage, 2);
                LbPercentage.Content = "Percentage - " + percentage.ToString() + " %";
            }
        }

        public void FillBoxes()
        {
            LbExam.Items.Clear();
            for (int i = 0; i < Class.Exams.Count; i++)
            {
                ListBoxItem item = new()
                {
                    Content = Class.Exams[i].Title + " " + Class.Exams[i].EndDate?.ToString("dd/MM/yyyy"),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed")
                };
                LbExam.Items.Add(item);
                item.FontSize = 17;
            }

            LbAssignment.Items.Clear();
            for (int i = 0; i < Class.Assignements.Count; i++)
            {
                ListBoxItem item = new()
                {
                    Content = Class.Assignements[i].Title + " " + Class.Assignements[i].EndDate?.ToString("dd/MM/yyyy"),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed"),
                    FontSize = 17
                };
                LbAssignment.Items.Add(item);
            }
        }

        private void BtClass1_Click(object sender, RoutedEventArgs e)
        {
            this.Class = User.Classes[0];
            FillBoxes();
            DisplayGradePoints();
            ChangeName();
            
        }

        private void BtClass2_Click(object sender, RoutedEventArgs e)
        {
            this.Class = User.Classes[1];
            FillBoxes();
            DisplayGradePoints();
            ChangeName();
            
        }

        private void BtClass3_Click(object sender, RoutedEventArgs e)
        {
            this.Class = User.Classes[2];
            FillBoxes();
            DisplayGradePoints();
            ChangeName();
            
        }

        private void BtClass4_Click(object sender, RoutedEventArgs e)
        {
            this.Class = User.Classes[3];
            FillBoxes();
            DisplayGradePoints();
            ChangeName();
            
        }

        private void BtClass5_Click(object sender, RoutedEventArgs e)
        {
            this.Class = User.Classes[4];
            FillBoxes();
            DisplayGradePoints();
            ChangeName();
            
        }

        private void BtAddTask_Click(object sender, RoutedEventArgs e)
        {
            DisplayNewFormular?.Invoke();
        }

        private void BtEditTask_Click(object sender, RoutedEventArgs e)
        {
            switch (focusedListBox)
            {
                case 0:
                    EditSelectedItem(Class.Assignements, LbAssignment);
                    break;
                case 1:
                    EditSelectedItem(Class.Exams,LbExam);
                    break;
                default:
                    break;
            }
        }

        public void ChangeType(ClassTask t, ClassTaskType? oldType, ClassTaskType? newType) {
            if (oldType == ClassTaskType.Exam && newType == ClassTaskType.Assignment) {
                Class.Exams.Remove(t);
                Class.Assignements.Add(t);

                User.Assignements.Add(t);
                User.Exams.Remove(t);
            } else if (oldType == ClassTaskType.Assignment && newType == ClassTaskType.Exam) {
                Class.Assignements.Remove(t);
                Class.Exams.Add(t);

                User.Assignements.Remove(t);
                User.Exams.Add(t);
            }
        }

        private void LbAssignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            focusedListBox = 0;
        }

        private void LbExam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            focusedListBox = 1;
        }

        private void BtAssignmentSortDate_Click(object sender, RoutedEventArgs e)
        {
            SortByDate(Class.Assignements, LbAssignment);
        }

        private void BtExamSortDate_Click(object sender, RoutedEventArgs e)
        {
            SortByDate(Class.Exams, LbExam);
        }


        public static void SortAZ(List<ClassTask> focusedList, ListBox focusedListBox) {
            DateTime today = DateTime.Today;
            List<ClassTask> sortedTaks = focusedList.OrderBy(t => t.Title).ToList();

            focusedListBox.Items.Clear();

            foreach (var task in sortedTaks)
            {
                ListBoxItem item = new()
                {
                    Content = task.Title + " " + task.EndDate?.ToString("dd/MM/yyyy"),
                    FontSize = 17,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed")
                };
                focusedListBox.Items.Add(item);
            }
        }

        public static void SortByDate(List<ClassTask> focusedList, ListBox focusedListBox) {
            DateTime today = DateTime.Today;
            List<ClassTask> TasksForToday = focusedList.Where(task => task.EndDate?.Date == today).OrderBy(task => task.EndDate).ToList();
            List<ClassTask> PastDueTasks = focusedList.Where(task => task.EndDate?.Date < today).OrderBy(task => task.EndDate).ToList();
            List<ClassTask> FutureTasks = focusedList.Where(task => task.EndDate?.Date > today).OrderBy(task => task.EndDate).ToList();

            focusedListBox.Items.Clear();

            foreach (var task in PastDueTasks)
            {
                ListBoxItem item = new()
                {
                    Content = task.Title + " " + task.EndDate?.ToString("dd/MM/yyyy"),
                    Foreground = new SolidColorBrush(Colors.Red),
                    FontSize = 17,
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed")
                };
                focusedListBox.Items.Add(item);
            }

            foreach (var task in TasksForToday)
            {
                ListBoxItem item = new()
                {
                    Content = task.Title + " " + task.EndDate?.ToString("dd/MM/yyyy"),
                    Foreground = new SolidColorBrush(Colors.Orange),
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed"),
                    FontSize = 17
                };
                focusedListBox.Items.Add(item);
            }

            foreach (var task in FutureTasks)
            {
                ListBoxItem item = new()
                {
                    Content = task.Title + " " + task.EndDate?.ToString("dd/MM/yyyy"),
                    Foreground = new SolidColorBrush(Colors.Green),
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed"),
                    FontSize = 17
                };
                focusedListBox.Items.Add(item);
            }

        }

        private void BtAssignmentsSortAZ_Click(object sender, RoutedEventArgs e)
        {
            SortAZ(Class.Assignements, LbAssignment);
        }

        private void BtExamSortAZ_Click(object sender, RoutedEventArgs e)
        {
            SortAZ(Class.Exams, LbExam);
        }

        private void BtAddGrade_Click(object sender, RoutedEventArgs e)
        {
            if (gradeFormularShown == false)
            {
                gradeFormularShown = true;
                GridGradeAddFormular.Visibility = Visibility.Visible;
            }
            else {
                gradeFormularShown = false;
                GridGradeAddFormular.Visibility = Visibility.Hidden;
            }
            
        }

        private void BtShowGrade_Click(object sender, RoutedEventArgs e)
        {
            DisplayGrades?.Invoke();
        }

        private void BtAddGradeList_Click(object sender, RoutedEventArgs e)
        {
            //Ako prist na to ci je input double je na zaklade tohto -> https://stackoverflow.com/questions/9823836/checking-if-a-variable-is-of-data-type-double
            if (TbGradeMax.Text != "MAX" && TbGradeGot.Text != "GOT")
            {
                bool isGotDouble = double.TryParse(TbGradeGot.Text, out double gotPoints);
                bool isMaxDouble = double.TryParse(TbGradeMax.Text, out double maxPoints);

                if (isGotDouble && isMaxDouble)
                {
                    Class.Grades.Add(new Grade(TbGradeTitle.Text, gotPoints, maxPoints));
                    TbGradeTitle.Foreground = new SolidColorBrush(Colors.White);
                    TbGradeMax.Foreground = new SolidColorBrush(Colors.White);
                    TbGradeGot.Foreground = new SolidColorBrush(Colors.White);
                    DisplayGradePoints();
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a number for 'Got' and 'Max'.");
                }
            }
            else{
                TbGradeTitle.Foreground = new SolidColorBrush(Colors.Red);
                TbGradeMax.Foreground = new SolidColorBrush(Colors.Red);
                TbGradeGot.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void BtDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            switch (focusedListBox)
            {
                case 0:
                    DeleteTask(Class.Assignements, LbAssignment);
                    break;
                case 1:
                    DeleteTask(Class.Exams, LbExam);
                    break;
                default:
                    break;
            }
        }

        public void DeleteTask(List<ClassTask> focusedList, ListBox focusedListBox) {
            int index = focusedListBox.SelectedIndex;
            if (index != -1) {
                ClassTask t = focusedList.ElementAt(index);
                focusedList.RemoveAt(index);
                if (focusedList == Class.Assignements) {
                    User.Assignements.Remove(t);
                } else if (focusedList == Class.Exams)
                {
                    User.Exams.Remove(t);
                }
                FillBoxes();
            }
        }
    }
}
