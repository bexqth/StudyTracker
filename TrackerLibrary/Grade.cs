namespace TrackerLibrary
{
    public class Grade
    {

        public string Title { get; set; }
        public double RecievedPoints { get; set; }
        public double MaxPoints { get; set; }

        public Grade(string title, double recievedPoints, double maxPoints) { 
            this.Title = title;
            this.RecievedPoints = recievedPoints;
            this.MaxPoints = maxPoints;
        }

    }
}