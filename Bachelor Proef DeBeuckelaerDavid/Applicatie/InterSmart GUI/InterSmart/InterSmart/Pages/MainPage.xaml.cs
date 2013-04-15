using InterSmart.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Windows.Threading;

namespace InterSmart.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        #region Veriabelen 
        public static getTweets tweet = new getTweets();
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        private ObservableCollection<Vragen> _Vragen = new ObservableCollection<Vragen>();
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        int var = -1; //voor popup
        #endregion 

        public MainPage()
        {
            InitializeComponent();
        }
        #region Knoppen openen andere pages
        private void BtnShowStats_Click(object sender, RoutedEventArgs e)
        {
            InterSmart.Pages.StatistiekenPage StatPage = new Pages.StatistiekenPage();
            StatPage.Show();
        }

        private void BtnShowFeedBack_Click(object sender, RoutedEventArgs e)
        {
            InterSmart.Pages.FeedBackPage FeedBackPage = new Pages.FeedBackPage();
            FeedBackPage.Show();
        }

        private void BtnShowDataBase_Click(object sender, RoutedEventArgs e)
        {
            InterSmart.Pages.DataBasePage DataBasePage = new Pages.DataBasePage();
            DataBasePage.Show();
        }
        #endregion

        #region Events voor Quiz feature
        private void BtnGetAntwoorden_Click(object sender, RoutedEventArgs e)
        {
             timer.Tick += timer_Tick;
             timer.Interval = new TimeSpan(0, 0, 0, 1);


            if (TextTweet.Text == "")
            {
                MessageBox.Show("Input Tweet Search !");
            }
            else
            {
                string Tweetzoek = TextTweet.Text;
                _tweets = tweet.GetCollection(Tweetzoek);
                TweetList.ItemsSource = _tweets;
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            string Tweetzoek;
            Tweetzoek = TextTweet.Text;
            _tweets = tweet.GetCollection(Tweetzoek);
            TweetList.ItemsSource = _tweets;
        }

        private void chbxRfrchAntwoorden_Checked_1(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void chbxRfrchAntwoorden_Unchecked_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void BtnClearAntwoorden_Click_1(object sender, RoutedEventArgs e)
        {

            chbxRfrchAntwoorden.IsChecked = false;
            tweet.add = true;
            tweet.alleTweets = 0;
            _tweets.Clear();
            TextTweet.Clear();

        }
        #endregion 

        #region Events voor vragen
        private void BtnGetVragen_Click(object sender, RoutedEventArgs e)
        {
            timer2.Tick += timer2_Tick;
            timer2.Interval = new TimeSpan(0, 0, 0, 1);


            if (txbxQuestionTweet.Text == "")
            {
                MessageBox.Show("Input Tweet Search !");
            }
            else
            {
                string TweetVragen = txbxQuestionTweet.Text;
                _Vragen = tweet.returnVragen(TweetVragen);
                VraagList.ItemsSource = _Vragen;
            }
        }

        public void timer2_Tick(object sender, EventArgs e)
        {
            string TweetVragen = txbxQuestionTweet.Text;
            _Vragen = tweet.returnVragen(TweetVragen);
            VraagList.ItemsSource = _Vragen;


            if (var != VraagList.Items.Count && VraagList.Items.Count != 0) //voor tweet pop-up te laten zien als lijst word upgedate
            {
                QuestionPopUp popUp = new QuestionPopUp();
                popUp.Show();
                var = VraagList.Items.Count;
            }
        }

        private void ChBxRefrechVragen_Checked_1(object sender, RoutedEventArgs e)
        {
            timer2.Start();
        }

        private void ChBxRefrechVragen_Unchecked_1(object sender, RoutedEventArgs e)
        {
            timer2.Stop();
        }

        private void BtnClearVragen_Click(object sender, RoutedEventArgs e)
        {
            ChBxRefrechVragen.IsChecked = false;
            tweet.add2 = true;
            tweet.alleTweets2 = 0;
            _Vragen.Clear();
            txbxQuestionTweet.Clear();
        }
        #endregion

    }
}
