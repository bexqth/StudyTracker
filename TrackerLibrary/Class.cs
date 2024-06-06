using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TrackerLibrary
{
    public class Class
    {
        //public string? Name { get; set; }
        public List<ClassTask> Exams { get; set; }
        public List<ClassTask> Assignements { get; set; }
        public List<Grade> Grades { get; set; }
        public string ClassName{get;set;}
        public double RecievedPoints { get; set; }
        public double AllPoints { get; set; }
        public Class(string ClassName) { 
            this.ClassName = ClassName;
            Exams = new List<ClassTask>();
            Assignements = new List<ClassTask>();
            Grades = new List<Grade>();
        }

        public void UpdatePoints() {
            RecievedPoints = 0;
            AllPoints = 0;
            foreach (var g in Grades) {
                RecievedPoints += g.RecievedPoints;
                AllPoints += g.MaxPoints;
            }
            
        }

        public double CalculatePercentage() {
            if (AllPoints == 0) {
                return 0;
            }
            return (RecievedPoints/AllPoints) * 100;       
        }

        public void AddTask(ClassTask classTask) {
            if (classTask.Type == ClassTaskType.Exam) {
                this.Exams.Add(classTask);
            } else if (classTask.Type == ClassTaskType.Assignment) {
                this.Assignements.Add(classTask);
            }
        
        }
    }
}
