using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ClassTaskFormularUserControl.xaml
    /// </summary>
    public partial class ClassTaskFormularUserControl : UserControl
    {

        public DateTime? dateEndDate { get; set; }
        public ClassTaskType type { get; set; }
        public ClassTaskFormularUserControl()
        {
            InitializeComponent();
        }

        public delegate void SaveEventHandler(); //DELEGAT
        public event SaveEventHandler SendSaveFormular; //UDALOST

        public void SaveFormular()
        {
            SendSaveFormular?.Invoke();
        }

        public delegate void CancelEventHandler(); //DELEGAT
        public event CancelEventHandler SendCancelFormular; //UDALOST


        public void CancelFormular()
        {
            SendCancelFormular?.Invoke();
        }


        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            DateTime? selectedDate = cCalendar.SelectedDate;
            if (selectedDate.HasValue)
            {
                DateTime date = selectedDate.Value;
                string formattedDate = date.ToString("dd/MM/yyyy");
                TbEndDate.Text = formattedDate;
                this.dateEndDate = selectedDate;
            }
        }

        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            SaveFormular();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelFormular();
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbType.SelectedIndex == 0)
            {
                this.type = ClassTaskType.Exam;
            }
            else if (CbType.SelectedIndex == 1)
            {
                this.type = ClassTaskType.Assignment;
            }
        }
    }
}
