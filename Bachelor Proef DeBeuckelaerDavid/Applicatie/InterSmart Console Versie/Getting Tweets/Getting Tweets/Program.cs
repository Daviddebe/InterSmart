using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Getting_Tweets
{
   public class Program
    {
        static getTweets tweet = new getTweets();
        static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
       static DataBase DataProg = new DataBase();

       public static void Main(string[] args)
       {
           DataProg.DataProg();
       //start:
       //    weergeven();
       //  if(_tweets.Count() <=0)
       //  {
       //      goto start;
       //  }
       //  else
       //  {
       //      Console.WriteLine("einde programma");
       //  }

       //  Console.WriteLine("Press \'q\' to quit the sample.");
       //  while (Console.Read() != 'q') ;

           //DataProg.DataProg();
           //DataProg.printtweet();
           //System.Timers.Timer timer = new System.Timers.Timer();
           //timer.Elapsed += timer_Elapsed;
           //timer.Interval = 3000;
           //timer.Enabled = true;

       }

       static void timer_Elapsed(object sender, ElapsedEventArgs e)
       {
           weergeven();
       }
       public static void weergeven()
       {
           _tweets = tweet.GetCollection();
           if (_tweets.Count() > 0)
           {
               Console.WriteLine("Tweets:");
               Console.WriteLine("---------------------------------------------------");
               Console.WriteLine("---------------------------------------------------");
               foreach (var t in _tweets)
               {
                   Console.WriteLine(t.Author.Name + " tweeted: " + t.Title + " at  " + t.TweetDate);
                   Console.WriteLine("---------------------------------------------------");
               }
               Console.WriteLine("---------------------------------------------------");
           }
       } //tweets weergeven op console

    }
}

