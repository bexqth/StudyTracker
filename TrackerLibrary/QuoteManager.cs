using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class QuoteManager
    {
        public List<Quote> Quotes;

        public QuoteManager() {
            this.Quotes = new List<Quote>();
        }

        public void LoadFromJson(FileInfo jsonFile) {
            string json = File.ReadAllText(jsonFile.FullName);
            if (json != null)
            {
                List<Quote>? quotes = JsonSerializer.Deserialize<List<Quote>>(json);
                if (quotes != null)
                {
                    this.Quotes = quotes;
                }
            }
        }

        public Quote RandomSelectQuote() {
            Random rng = new();
            int randomNumber = rng.Next(this.Quotes.Count);
            return this.Quotes[randomNumber];
        }

    }
}
