using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;

namespace InterSmartV1._1.Menu
{
   public class getTweets
    {
        // 
       public string Uri = "http://search.twitter.com/search.atom?q=";
       public string UriTot;
       private List<int> AnswerList = new List<int>();

     

       public getTweets(string zoekstring)
       {
           UriTot = Uri + zoekstring + "&:)&since:year-month-date&rpp=1500"; //volledige Twitter API met zoeksting in het midden.
           show();
       }
     
       private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
       WebClient client = new WebClient();//methode's voor ontvangen en zenden van data van een recourse geidentifiseerd door een URI

       public void show()
       {
           
            client.DownloadStringCompleted += (s, ea) => //als de download compleet is (met 2 parameters)
            {
                    XDocument doc = XDocument.Parse(ea.Result); //XML Document
                    XNamespace ns = "http://www.w3.org/2005/Atom"; //namespace toevoegen

                    var items = from item in doc.Descendants(ns + "entry")
                                select new Tweet()
                                {
                                    Title = item.Element(ns + "title").Value,

                                    Image = new Uri((from XElement xe in item.Descendants(ns + "link")
                                                     where xe.Attribute("type").Value == "image/png"
                                                     select xe.Attribute("href").Value).First<string>()),
                                    Author = new Author()
                                    {
                                        Name = item.Element(ns + "author").Element(ns + "name").Value,
                                        Uri = item.Element(ns + "author").Element(ns + "uri").Value
                                    }
                                    // De Linq Query zoekt door het document en zoekt al de entries er uit.
                                    // voor elke entry de titel,link, image type
                                };
                    foreach (Tweet t in items)
                    {
                        _tweets.Add(t); //all de tweets worden aan collection _tweet toegevoegt.

                    }
            };
            //aanroepen Twitter API

            client.DownloadStringAsync(new Uri(UriTot)); //laat zien welke tweets juist moeten worden weergegeven.
            
       }
       public List<int> GetAnswers() 
       {
           int A = 0;
           int B = 0;
           int C = 0;
           int D = 0;
           int totaal = 0; //geldige tweets
           int alleTweets = 0;
           foreach (var t in _tweets) //zal kijken in de tweets of er A of =A aanwezig is om zo de antwoorden er uit te halen
           {
               alleTweets += 1;

               if (t.Title.Contains(" A") || t.Title.Contains("=A")) //nu haal ik per "string" uit, als ik een app voor twitter maak kan ik exeptions toepassen waardoor er minder fouten zulle ontstaan
               {
                   A += 1;
                   totaal += 1;
               }
               else if (t.Title.Contains(" B") || t.Title.Contains("=B"))
               {
                   B += 1;
                   totaal += 1;
               }
               else if (t.Title.Contains(" C") || t.Title.Contains("=C"))
	           {
                   C += 1;
                   totaal += 1;
	           }
               else if (t.Title.Contains(" D") || t.Title.Contains("=D"))
               {
                   D += 1;
                   totaal += 1;
               }
           }
           AnswerList.Add(A);
           AnswerList.Add(B);
           AnswerList.Add(C);
           AnswerList.Add(D);
           AnswerList.Add(totaal);
           AnswerList.Add(alleTweets);
           return AnswerList;
       }

       public ObservableCollection<Tweet> GetCollection()
       {
           return _tweets;
       }

       
    }
}
