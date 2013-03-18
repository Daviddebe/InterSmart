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
       public string vraaguri;
       private List<int> AnswerList = new List<int>();
       public int alleTweets = 0;
       public int alleTweets2 = 0;
       public bool add = true;
       public bool add2 = true;
     
       private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
       private ObservableCollection<Vragen> _Vragen = new ObservableCollection<Vragen>();
       WebClient client = new WebClient();//methode's voor ontvangen en zenden van data van een recourse geidentifiseerd door een URI
       WebClient client2 = new WebClient();
       public void getVragen() //om te vragen binnen te krijgen
       {
           try
           {
               client2.DownloadStringAsync(new Uri(vraaguri)); //laat zien welke tweets juist moeten worden weergegeven.
           }
           catch (Exception) //exc er bij zette moest fout nog toeslagen
           {

           }
           client2.DownloadStringCompleted += (s, ea) => //als de download compleet is (met 2 parameters)
           {
                   XDocument doc = XDocument.Parse(ea.Result); //XML Document
                   XNamespace ns = "http://www.w3.org/2005/Atom"; //namespace toevoegen

                   var items = from item in doc.Descendants(ns + "entry")
                               select new Vragen()
                               {
                                   Title = item.Element(ns + "title").Value,

                                   Image = new Uri((from XElement xe in item.Descendants(ns + "link")
                                                    where xe.Attribute("type").Value == "image/png"
                                                    select xe.Attribute("href").Value).First<string>()),
                                   Author = new Author()
                                   {
                                       Name = item.Element(ns + "author").Element(ns + "name").Value,
                                       Uri = item.Element(ns + "author").Element(ns + "uri").Value
                                   },
                                  
                                   // De Linq Query zoekt door het document en zoekt al de entries er uit.
                                   // voor elke entry de titel,link, image type
                               };
                   if (items.Count() > alleTweets2)
                   {
                       List<Vragen> templist = new List<Vragen>();
                       foreach (Vragen t in items)
                       {
                           templist.Add(t);
                       }
                       for (int i = alleTweets2; i < items.Count(); i++)
                       {
                           if (add2 == true)
                           {
                               _Vragen.Add(templist[i]);
                               templist[i].Ontvangen = DateTime.Now;
                           }
                           else
                           {
                               _Vragen.Insert(0, templist[items.Count() - i - 1]);
                               templist[items.Count() - i - 1].Ontvangen = DateTime.Now;
                           }
                           alleTweets2 += 1;
                       }
                       add2 = false;
                   }
               //foreach(Vragen t in items)
               //{
               //    _Vragen.Add(t);
               //}
           };
          
       }
       public ObservableCollection<Vragen> returnVragen(string zoekstring)
       {
           vraaguri = Uri + zoekstring + "&:)&since:year-month-date&rpp=1500";
           getVragen();
           return _Vragen;
       }
       public void show()
       {
           try
           { //aanroepen Twitter API
               client.DownloadStringAsync(new Uri(UriTot)); //laat zien welke tweets juist moeten worden weergegeven.
           }
           catch (Exception) //exc er bij zette moest fout nog toeslagen
           {

           }
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
                                    },
                                    TweetDate = item.Element(ns + "published").Value
                                    // De Linq Query zoekt door het document en zoekt al de entries er uit.
                                    // voor elke entry de titel,link, image type
                                };
                    if (items.Count() > alleTweets)
                    {
                        List<Tweet> templist = new List<Tweet>();
                        foreach (Tweet t in items)
                        {
                            templist.Add(t);
                        }
                        for (int i = alleTweets; i < items.Count(); i++)
                        {
                            if (add == true)
	                        {
		                        _tweets.Add(templist[i]);
                                templist[i].Ontvangen = DateTime.Now;
	                        }
                            else
                            {
                                _tweets.Insert(0, templist[items.Count()-i-1]);
                                templist[items.Count()-i-1].Ontvangen = DateTime.Now;
                            }
                            alleTweets += 1;
                        }
                        add = false;
                    }
            };
           
           

       }
       public List<int> GetAnswers() 
       {
           int A = 0;
           int B = 0;
           int C = 0;
           int D = 0;
           int totaal = 0; //geldige tweets
           
           foreach (var t in _tweets) //zal kijken in de tweets of er A of =A aanwezig is om zo de antwoorden er uit te halen
           {
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

       public ObservableCollection<Tweet> GetCollection(string zoekstring)
       {
           UriTot = Uri + zoekstring + "&:)&since:year-month-date&rpp=1500"; //volledige Twitter API met zoeksting in het midden.
           show();
           return _tweets;
       }

       
    }
}
