using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Getting_Tweets
{
    public class getTweets
    {
        #region Variable voor Quiz
        public string UriTot = "http://search.twitter.com/search.atom?q=Inter_Smart&:)&since:year-month-date&rpp=100";
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        WebClient client = new WebClient(); //methode voor ontvangen/zenden van data van een resourse geidentifiseerd door een URI
        public int alleTweets = 0;
        public bool add = true;
        #endregion

        #region Quiz feature
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
                            _tweets.Insert(0, templist[items.Count() - i - 1]);
                            templist[items.Count() - i - 1].Ontvangen = DateTime.Now;
                        }
                        alleTweets += 1;
                    }
                    add = false;
                }
            };
        }
        public ObservableCollection<Tweet> GetCollection()
        {
            show();
            return _tweets;
        }
        #endregion 
    }
}
