using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackerLibrary
{
    public class User
    {
        public string? Name { get; set; }
        public List<ClassTask> Exams { get; set; }
        public List<ClassTask> Assignements { get; set; }
        public List<Task> ToDos { get; set; }
        public int ToDoTasksDone { get; set; }
        public int AllToDoTaks { get; set; }
        public List<StudySession> StudySessions { get; set; }

        public List<Class> Classes { get; set; }
        public User()
        {
            this.StudySessions = new List<StudySession>();
            this.Assignements = new List<ClassTask>();
            this.Exams = new List<ClassTask>();
            this.ToDos = new List<Task> { };
            this.Classes = new List<Class>();
            ToDoTasksDone = 0;
            AllToDoTaks = 0;
        }

        public void AddStudySession(StudySession session) {
            this.StudySessions.Add(session);
        }


        public void SaveInCSV() {
            string filePath1 = "files/studySessions.csv";
            var csvContent1 = new StringBuilder();
            foreach (var session in StudySessions)
            {
                string duration = session.Duration.ToString();
                string? date = session.Date?.ToString("yyyy-MM-dd");
                string line = $"{duration},{date}";
                csvContent1.AppendLine(line);
            }
            File.WriteAllText(filePath1, csvContent1.ToString());

            string filePath2 = "files/ClassTasks.csv";
            var csvContent2 = new StringBuilder();
            for (int i = 0; i < this.Classes.Count; i++)
            {
                for (int j = 0; j < this.Classes[i].Exams.Count; j++)
                {
                    string className = this.Classes[i].ClassName;
                    string? endDate = this.Classes[i].Exams[j].EndDate?.ToString("yyyy-MM-dd");
                    string? title = this.Classes[i].Exams[j].Title;
                    string type = "Exam";
                    string line = $"{className},{endDate},{title},{type}";
                    csvContent2.AppendLine(line);
                }
                for (int k = 0; k < this.Classes[i].Assignements.Count; k++)
                {
                    string className = this.Classes[i].ClassName;
                    string? endDate = this.Classes[i].Assignements[k].EndDate?.ToString("yyyy-MM-dd");
                    string? title = this.Classes[i].Assignements[k].Title;
                    string type = "Assignment";
                    string line = $"{className},{endDate},{title},{type}";
                    csvContent2.AppendLine(line);
                }
            }
            File.WriteAllText(filePath2, csvContent2.ToString());

        }

        public void LoadAllDataFromCSV()
        {           
            using (var reader = new StreamReader("files/studySessions.csv"))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    int duration = int.Parse(values[0]);
                    DateTime? date = string.IsNullOrWhiteSpace(values[1]) ? (DateTime?)null : DateTime.Parse(values[1]);
                    var session = new StudySession(duration, date);
                    StudySessions.Add(session);
                }
            }

            using (var reader = new StreamReader("files/Classes.csv"))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var className = line.Trim();
                    var classInfo = new Class(className);
                    Classes.Add(classInfo);
                }
            }

            using (var reader = new StreamReader("files/ClassTasks.csv"))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    var className = parts[0].Trim();
                    var date = DateTime.Parse(parts[1]);
                    var taskName = parts[2].Trim();
                    string taskType = parts[3].Trim();
                    ClassTaskType c;
                    ClassTask? classTask = null;
                    if (taskType == "Exam")
                    {
                        c = ClassTaskType.Exam;
                        classTask = new ClassTask(taskName, date, c);
                        this.Exams.Add(classTask);
                    }
                    else {
                        c = ClassTaskType.Assignment;
                        classTask = new ClassTask(taskName, date, c);
                        this.Assignements.Add(classTask);
                    }


                    foreach (var classInfo in Classes)
                    {
                        if (classInfo.ClassName == className)
                        {
                            classInfo.AddTask(classTask);
                            break;
                        }
                    }
                }

            }

        }
    }
}
