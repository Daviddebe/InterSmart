using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InterSmart_Beta.Class
{
    public class getTweets
    {
        #region Variable voor Ontvangen van Tweets

        public string UriTot = "";
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        WebClient client = new WebClient(); //methode voor ontvangen/zenden van data van een resourse geidentifiseerd door een URI
        public int alleTweets = 0;
        public bool add = true;
        int pagina = 1; //de pagina 

        #endregion

        #region Ontvangen Tweets
        public ObservableCollection<Tweet> show()
        {
            UriTot = "http://search.twitter.com/search.atom?q=Inter_Smart&rpp=100&page="+ pagina.ToString();
            try
            { //aanroepen Twitter API
                    client.DownloadStringAsync(new Uri(UriTot)); //laat zien welke tweets juist moeten worden weergegeven.
            }
            catch (Exception) //exc er bij zette moest fout nog toeslagen
            {

            }
            client.DownloadStringCompleted += (s, ea) => //als de download compleet is (met 2 parameters)
            {
                try
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
                
                    if (pagina <=2)
                    {
                        
                        foreach (Tweet t in items)
                        {
                            _tweets.Add(t);
                            //Console.WriteLine(t.Title + " totaal:" + _tweets.Count.ToString() + " pagina: " + pagina.ToString());
                        }
                        pagina += 1;
                        show();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Still no internet, application will close down!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            };
            return _tweets;
        }
        public ObservableCollection<Tweet> GetCollection()
        {
            UriTot = "http://search.twitter.com/search.atom?q=Inter_Smart&rpp=100&page=1";
            show();
            return _tweets; 
        }
        #endregion 

    }
}
