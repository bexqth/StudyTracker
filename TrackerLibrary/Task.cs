using System.ComponentModel;

namespace TrackerLibrary
{
    public class Task
    {

        public string? Title { get; set; }

        public Task(string? Title)
        {
            this.Title = Title;
        }


    }
}
