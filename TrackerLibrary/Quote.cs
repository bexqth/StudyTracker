using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class Quote
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public Quote(string text, string author) { 
            this.Text = text;
            this.Author = author;
        }

    }
}
