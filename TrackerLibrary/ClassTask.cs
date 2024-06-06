using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class ClassTask : Task
    {
        public ClassTaskType Type { get; set; }
        public DateTime? EndDate { get; set; }
        public ClassTask(string? Title, DateTime? EndDate, ClassTaskType Type) : base(Title)
        {
            this.Type = Type;
            this.EndDate = EndDate;
        }
    }
}
