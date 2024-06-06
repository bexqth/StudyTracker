using System.Collections.ObjectModel;
using System.IO;
using System.Text;
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

namespace Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TimerWindow timerWindow;
        private readonly DashboardUserControl dashboard;
        private readonly User User;
        private readonly StatsUserControl stats;
        private readonly ClassesUserControl classes;
        private readonly ToDoUserControl todo;
        private ClassTask EditedClassTask;
        private readonly ClassTaskFormularUserControl ClassTaskFormular;
        private readonly GradeUserControl Grades;
        private ClassTaskType? oldState = null;
        private ClassTaskType? newState = null;
        public MainWindow()
        {
            InitializeComponent();
            timerWindow = new TimerWindow();
            dashboard = new DashboardUserControl();
            stats = new StatsUserControl();
            classes = new ClassesUserControl();
            todo = new ToDoUserControl();
            ClassTaskFormular = new ClassTaskFormularUserControl();
            Grades = new GradeUserControl();

            User = new User();
            User.LoadAllDataFromCSV();
            
            timerWindow.User = User;
            todo.User = User;
            dashboard.User = User;
            stats.User = User;
            classes.User = User;
            classes.DisplayClasses();
            classes.DisplayGradePoints();

            classes.DisplayEditFormular += DisplayEditFormular;
            classes.DisplayNewFormular += DisplayNewFormular;
            classes.DisplayGrades += DisplayGrades;

            dashboard.UpdateStats();
            stats.LoadData();
            stats.RbWeek.IsChecked = true;
            stats.ShowGraph(1);
            CenterFrame.Navigate(dashboard);
        }

        public void DisplayGrades() {
            Grades.SendCancelGrades += CancelGrades;
            Grades.UpdateEditGradeFormular(classes.Class.ClassName, classes.Class.AllPoints, classes.Class.RecievedPoints);
            Grades.DisplayGrades(classes.Class);
            CenterFrame.Navigate(Grades);
            
        }

        public void CancelGrades() {
            classes.DisplayGradePoints();
            CenterFrame.Navigate(classes);
        }
        
        private void DisplayEditFormular(List<ClassTask> focusedList, ListBox focusedListBox)
        {
            if (focusedListBox.SelectedIndex != -1) {
                ClassTaskFormular.SendSaveFormular += ClassTaskSaveEditedFormular;
                ClassTaskFormular.SendCancelFormular += ClassTaskCancelFormular;

                EditedClassTask = focusedList.ElementAt(focusedListBox.SelectedIndex);
                ClassTaskFormular.TbEndDate.Text = EditedClassTask.EndDate.ToString();
                ClassTaskFormular.TbTitle.Text = EditedClassTask.Title;
                if (EditedClassTask.Type == ClassTaskType.Exam)
                {
                    ClassTaskFormular.CbType.SelectedIndex = 0;
                    oldState = ClassTaskType.Exam;
                }
                else if (EditedClassTask.Type == ClassTaskType.Assignment)
                {
                    ClassTaskFormular.CbType.SelectedIndex = 1;
                    oldState = ClassTaskType.Assignment;
                }
                CenterFrame.Navigate(ClassTaskFormular);
            }
        }

        public void DisplayNewFormular() {
            ClassTaskFormular.SendSaveFormular += ClassTaskSaveNewFormular;
            ClassTaskFormular.SendCancelFormular += ClassTaskCancelFormular;
            ClassTaskFormular.TbEndDate.Text = null;
            ClassTaskFormular.TbTitle.Text = null;
            ClassTaskFormular.CbType.SelectedIndex = 0;

            CenterFrame.Navigate(ClassTaskFormular);           
            classes.FillBoxes();
            //User.SaveInCSV();
        }

        public void ClassTaskSaveNewFormular()
        {
            ClassTaskFormular.SendSaveFormular -= ClassTaskSaveNewFormular;
            ClassTaskFormular.SendCancelFormular -= ClassTaskCancelFormular;

            ClassTask newClassTask = new(ClassTaskFormular.TbTitle.Text, ClassTaskFormular.dateEndDate, ClassTaskFormular.type);
            classes.Class.AddTask(newClassTask);
            classes.FillBoxes();
            CenterFrame.Navigate(classes);
        }

        public void ClassTaskSaveEditedFormular()
        {
            EditedClassTask.EndDate = DateTime.Parse(ClassTaskFormular.TbEndDate.Text);
            EditedClassTask.Title = ClassTaskFormular.TbTitle.Text;
            if (ClassTaskFormular.CbType.SelectedIndex == 0)
            {
                EditedClassTask.Type = ClassTaskType.Exam;
                newState = ClassTaskType.Exam;
            }
            else if (ClassTaskFormular.CbType.SelectedIndex == 1)
            {
                EditedClassTask.Type = ClassTaskType.Assignment;
                newState = ClassTaskType.Assignment;
            }

            ClassTaskFormular.SendSaveFormular -= ClassTaskSaveEditedFormular;
            ClassTaskFormular.SendCancelFormular -= ClassTaskCancelFormular;

            classes.ChangeType(EditedClassTask, oldState, newState);
            classes.FillBoxes();
            CenterFrame.Navigate(classes);
        }

        public void ClassTaskCancelFormular() {
            CenterFrame.Navigate(classes);
        }

        private void BtTimerClick(object sender, RoutedEventArgs e)
        {
            CenterFrame.Navigate(timerWindow);
        }

        private void BtDashboardClick(object sender, RoutedEventArgs e)
        {
            dashboard.UpdateStats();
            CenterFrame.Navigate(dashboard);
        }

        private void BtStats_Click(object sender, RoutedEventArgs e)
        {
            stats.LoadData();
            stats.RbWeek.IsChecked = true;
            stats.ShowGraph(1);
            CenterFrame.Navigate(stats);
        }

        private void BtClasses_Click(object sender, RoutedEventArgs e)
        {
            CenterFrame.Navigate(classes);
        }

        private void BtToDo_Click(object sender, RoutedEventArgs e)
        {
            CenterFrame.Navigate(todo);
        }
    }
}