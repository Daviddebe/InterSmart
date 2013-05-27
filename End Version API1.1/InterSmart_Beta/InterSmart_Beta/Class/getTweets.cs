using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TweetSharp;

namespace InterSmart_Beta.Class
{
    public class getTweets
    {
        #region Variable voor Quiz
        public ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        WebClient client = new WebClient(); //methode voor ontvangen/zenden van data van een resourse geidentifiseerd door een URI
        public int alleTweets = 0;
        public bool add = true;
        #endregion

        #region Quiz feature

        public void show()
        {
            string comsumerkey = "9AjgguAcILz3gomklsJpxw";
            string consumersecret = "kQsd6pXk0eZUTKJe4aontxHgJE9LuzODJgwTZ03Kg";

            string _accestoken = "1096538353-ZGVk2iG9egRavKo73J3rZ7xEjV8C7YqWCHNB3SI";
            string _accessTokenSecret = "nJnqV71cFCpmhAO3ga7dkl0PuEjlzUX0aFx8K8w";

            var service = new TwitterService(comsumerkey, consumersecret);
            service.AuthenticateWith(_accestoken, _accessTokenSecret);

            var tweets = service.Search(new SearchOptions { Q = "@Inter_Smart", Count = 100 });

            foreach (var tweet in tweets.Statuses)
            {
                Tweet tempTweet = new Tweet();
                tempTweet.Title = tweet.Text;
                tempTweet.Author = new Author()
                {
                    Name = tweet.User.ScreenName
                };
                _tweets.Add(tempTweet);
                tempTweet.TweetDate = tweet.CreatedDate;
               
            }
        }
        public ObservableCollection<Tweet> GetCollection()
        {
            show();
            return _tweets;
        }
        #endregion 

    }
}
