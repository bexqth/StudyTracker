using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{

    
    public class User
    {
        public string? Name { get; set; }
        public List<Task> Exams { get; set; }
        public List<Task> Assignements { get; set; }

        public User() { 
        
            
        }

    }
}
