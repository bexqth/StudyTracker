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

namespace Tracker
{
    /// <summary>
    /// Interaction logic for GradeUserControl.xaml
    /// </summary>
    public partial class GradeUserControl : UserControl
    {
        private bool EditGradeFormularVisible = false;
        //private readonly List<Grade> ClassGrades;
        private Class actualClass;
        public GradeUserControl()
        {
            InitializeComponent();
        }

        public delegate void CancelGradesEventHandler(); //DELEGAT
        public event CancelGradesEventHandler SendCancelGrades; //UDALOST

        private void BtCancelGrades_Click(object sender, RoutedEventArgs e)
        {
            SendCancelGrades?.Invoke();
        }


        public void DisplayGrades(Class c) {
            actualClass = c;
            LbGrades.Items.Clear();
            for (int i = 0; i < c.Grades.Count; i++) {
                ListBoxItem item = new()
                {
                    Content = c.Grades[i].Title + " | Points: " + c.Grades[i].RecievedPoints.ToString() + " / " + c.Grades[i].MaxPoints.ToString(),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontFamily = new System.Windows.Media.FontFamily("Bahnschrift SemiLight SemiCondensed"),
                    FontSize = 17
                };
                LbGrades.Items.Add(item);
            }
        }

        public void HandleEditFormular() {
            if (EditGradeFormularVisible == false)
            {
                EditGradeFormularVisible = true;
                GridEditGradeFormular.Visibility = Visibility.Visible;
            }
            else
            {
                EditGradeFormularVisible = false;
                GridEditGradeFormular.Visibility = Visibility.Hidden;
            }
        }


        private void BtEditGrade_Click(object sender, RoutedEventArgs e)
        {
            HandleEditFormular();
        }

        private void BtConfirmChanges_Click(object sender, RoutedEventArgs e)
        {
            int index;
            if (LbGrades.SelectedIndex == -1)
            {
                EditGradeFormularVisible = false;
                GridEditGradeFormular.Visibility = Visibility.Hidden;
            }
            else
            {
                index = LbGrades.SelectedIndex;
                EditGradeFormularVisible = true;
                GridEditGradeFormular.Visibility = Visibility.Visible;

                Grade EditedGrade = actualClass.Grades.ElementAt(index);
                EditedGrade.Title = TbGradeTitle.Text;

                //Ako prist na to ci je input double je na zaklade tohto -> https://stackoverflow.com/questions/9823836/checking-if-a-variable-is-of-data-type-double
                bool isRecievedPointsDouble = double.TryParse(TbGradeGot.Text, out double recievedPoints);
                if (isRecievedPointsDouble)
                {
                    EditedGrade.RecievedPoints = recievedPoints;
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a number.");
                }

                bool isMaxPointsDouble = double.TryParse(TbGradeMax.Text, out double maxPoints);
                if (isMaxPointsDouble)
                {
                    EditedGrade.MaxPoints = maxPoints;
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a number.");
                }

                actualClass.UpdatePoints();

                UpdateEditGradeFormular(EditedGrade.Title, actualClass.AllPoints, actualClass.RecievedPoints);
                DisplayGrades(actualClass);
                LbGrades.SelectedIndex = index;


                UpdateEditGradeFormular(EditedGrade.Title, actualClass.AllPoints, actualClass.RecievedPoints);
                DisplayGrades(actualClass);
                LbGrades.SelectedIndex = index;
            }
        }

        private void LbGrades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index;
            if (LbGrades.SelectedIndex == -1)
            {
                index = 0;
                EditGradeFormularVisible = false;
                GridEditGradeFormular.Visibility = Visibility.Hidden;
            }
            else {
                index = LbGrades.SelectedIndex;
                EditGradeFormularVisible = true;
                GridEditGradeFormular.Visibility = Visibility.Visible;
            }
            Grade EditedGrade = actualClass.Grades.ElementAt(index);
            TbGradeTitle.Text = EditedGrade.Title;
            TbGradeGot.Text = EditedGrade.RecievedPoints.ToString();
            TbGradeMax.Text = EditedGrade.MaxPoints.ToString();
        }

        public void UpdateEditGradeFormular(string className, double allPoints, double recievedPoints)
        {
            LbClassName.Content = className;
            LbMaxPoints.Content = "Max points - " + allPoints.ToString();
            LbRecievedPoints.Content = "Recieved points - " + recievedPoints.ToString();
            double percentage = (recievedPoints / allPoints) * 100;
            percentage = Math.Round(percentage, 2);
            LbPercentage.Content = "Percentage - " + percentage.ToString() + " %";
        }
    }
}
