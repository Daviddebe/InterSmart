using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace InterSmartV1._1.Menu
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        //resultaat wordt in collection geload
        public static getTweets tweet;
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        public List<int> AnswerList = new List<int>();
       
        
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
            
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) //listbox binden
        {
            TweetList.ItemsSource = _tweets;
        }

        private void GetTweets_Click(object sender, RoutedEventArgs e)
        {
            if (TextTweet.Text == "")
            {
                MessageBox.Show("Input Tweet Search !");
            }
            else
            {
                string Tweetzoek = TextTweet.Text;
                tweet = new getTweets(Tweetzoek); //wordt ingegeven text
                _tweets = tweet.GetCollection();
                TweetList.ItemsSource = _tweets;
                
              
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) //alles clearen
        {
            
            TextTweet.Clear();
            while (_tweets.Count > 0)
            {
                _tweets.RemoveAt(_tweets.Count - 1);
            } // denk er aan als de statiestieken worden gesloten deze ook volledig gecleared worden.
        }

        private void BtnStats_Click(object sender, RoutedEventArgs e)
        {
            InterSmartV1._1.Menu.Statistieken WinStats = new Menu.Statistieken();
            WinStats.Show();
            
        }

        
    }
}
