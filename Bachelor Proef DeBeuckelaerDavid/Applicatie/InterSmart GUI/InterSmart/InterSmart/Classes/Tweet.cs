using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterSmart.Classes
{
   public class Tweet
    {
        public string Title { get; set; }
        public Uri Image { get; set; }
        public Uri Link { get; set; }
        public Author Author { get; set; }
        public DateTime Ontvangen { get; set; }
        public string TweetDate { get; set; }
    }
}
