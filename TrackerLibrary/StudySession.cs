using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class StudySession
    {
        public int Duration { get; set; }
        public DateTime? Date { get; set; }

        public StudySession(int duration, DateTime? date) { 
            this.Duration = duration;
            this.Date = date;
        }   

    }
}
