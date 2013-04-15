using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Getting_Tweets
{
   public class Program
    {
       public static void Main(string[] args)
       {
           weergeven();
       }

       public static void weergeven()
       {
           getTweets tweet = new getTweets();
           ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
           _tweets = tweet.GetCollection();

           Timer timer = new Timer(ComputeBoundOp, 5, 0, 1000);
           Console.WriteLine("Tweets:");
           Console.ReadLine();

           foreach (var t in _tweets)
           {
               Console.WriteLine(t.Title);
           }
           Console.ReadLine();
       }

       public static void ComputeBoundOp(Object state)
       {
           weergeven();
       }
    }
}
