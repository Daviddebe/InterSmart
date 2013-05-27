using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterSmart_Beta.Class
{
    public class Tweet
    {
        public string Title { get; set; }
        public int vraagNr = 100; 
        public int EvaluatieVraag = 100; 
        public Uri Image { get; set; }
        public Uri Link { get; set; }
        public Author Author { get; set; }
        public DateTime Ontvangen { get; set; }
        public DateTime TweetDate { get; set; }
    }
}
