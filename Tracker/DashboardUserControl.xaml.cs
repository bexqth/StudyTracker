using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for DashboardUserControl.xaml
    /// </summary>
    public partial class DashboardUserControl : UserControl
    {
        private readonly QuoteManager QuoteManager;
        public User User { get; set; }
        public DashboardUserControl()
        {
            InitializeComponent();
            QuoteManager = new QuoteManager();
            FileInfo jsonFile = new("files/quotes.json");
            QuoteManager.LoadFromJson(jsonFile);
            SetQuote();
            //meno pocitaca - https://stackoverflow.com/questions/1768198/how-do-i-get-the-computer-name-in-net
            lbName.Content = "Hello, " + Environment.UserName;
        }

        public void SetQuote()
        {
            var q = QuoteManager.RandomSelectQuote();
            TbQuoteText.Text = q.Text;
            TbQuoteAuthor.Text = q.Author;
        }

        public void UpdateStats() {
            TbAssignementCount.Text = User.Assignements.Count.ToString();
            TbExamCount.Text = User.Exams.Count.ToString();
            TbStudySessionCount.Text = User.StudySessions.Count(d => d.Date.HasValue && d.Date.Value == DateTime.Today).ToString();
            TbToDoTaskCount.Text = User.ToDos.Count.ToString();
            TbTaksDone.Text = "Done: " + User.ToDoTasksDone.ToString();
            TbTasksAll.Text = "All tasks: " + User.AllToDoTaks.ToString();
            if (User.AllToDoTaks == 0){
                PbToDoTaks.Value = 0;
            }
            else {
                int done = User.ToDoTasksDone;
                int all = User.AllToDoTaks;
                PbToDoTaks.Value = ((double)User.ToDoTasksDone / User.AllToDoTaks) * 100;
            }           
        }
    }
}
