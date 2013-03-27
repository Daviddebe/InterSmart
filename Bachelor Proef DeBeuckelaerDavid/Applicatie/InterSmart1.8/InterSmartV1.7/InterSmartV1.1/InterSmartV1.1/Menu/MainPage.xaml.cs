using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace InterSmartV1._1.Menu
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        //resultaat wordt in collection geload
        public static getTweets tweet = new getTweets();
        public static int deTweets = 0;
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        private ObservableCollection<Vragen> _Vragen = new ObservableCollection<Vragen>();
        public List<int> AnswerList = new List<int>();
        DispatcherTimer timer = new DispatcherTimer();
        private int count = 0;
        int var = -1; //voor popup
        

        public MainPage()
        {
            InitializeComponent();
           
        }


        public void GetTweets_Click(object sender, RoutedEventArgs e)//timer gebruikt zodat er automatich word ververst.
        {

            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);


            if (TextTweet.Text == "" || txbxQuestionTweet.Text == "")
            {
                MessageBox.Show("Input Tweet Search !");
            }
            else
            {
                string Tweetzoek = TextTweet.Text;
                _tweets = tweet.GetCollection(Tweetzoek);
                TweetList.ItemsSource = _tweets;

                string TweetVragen = txbxQuestionTweet.Text;
                _Vragen = tweet.returnVragen(TweetVragen);
                VraagList.ItemsSource = _Vragen;
            }
         
        }
        public void timer_Tick(object sender, EventArgs e) 
        {
            string Tweetzoek;
            string TweetVragen;
            if (count %2 == 0)
            {
                //if (vragenLijst.Count > 0)
                //{
                //    QuestionPopUp popUp = new QuestionPopUp();
                //    popUp.deVraag = vragenLijst.First().Title;
                //    popUp.Show();
                //}
                TweetVragen = txbxQuestionTweet.Text;
                _Vragen = tweet.returnVragen(TweetVragen);
               
                if (var != VraagList.Items.Count && VraagList.Items.Count != 0) //voor tweet pop-up te laten zien als lijst word upgedate
                {
                    QuestionPopUp popUp = new QuestionPopUp();
                    popUp.Show();
                    var = VraagList.Items.Count;
                }
            }
            else
            {
                Tweetzoek = TextTweet.Text;
                _tweets = tweet.GetCollection(Tweetzoek);
            }
           
            TweetList.ItemsSource = _tweets;
            VraagList.ItemsSource = _Vragen;
            count++;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) //alles clearen
        {
            tweet.add = true;
            tweet.alleTweets = 0;
            _tweets.Clear();
            _Vragen.Clear();
            rBtnRefr.IsChecked = false;
            TextTweet.Clear();
            txbxQuestionTweet.Clear();
            //while (_tweets.Count > 0)  knop bijmaken om lijst te clearen
            //{
            //    _tweets.RemoveAt(_tweets.Count - 1);
            //} // denk er aan als de statiestieken worden gesloten deze ook volledig gecleared worden.
        }

        private void BtnStats_Click(object sender, RoutedEventArgs e)
        {
            InterSmartV1._1.Menu.Statistieken WinStats = new Menu.Statistieken();
            WinStats.Show();

        }

        private void rBtnRefr_Checked_2(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void rBtnRefr_Unchecked_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void knopZien_Click(object sender, RoutedEventArgs e) //database aanmaken enkel van binnekomde tweets nog geen vragen fzo
        {
             
            XmlWriterSettings xwriter = new XmlWriterSettings();
            xwriter.Indent = true; ;
            xwriter.OmitXmlDeclaration = true;
            xwriter.Encoding = Encoding.ASCII;
            string path = @"TweetDB\" + TextTweet.Text.ToString() + ".xml"; //lokatie van opslaan
        
            using (XmlWriter writer = XmlWriter.Create(path, xwriter))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("XMLFILE");
          
            foreach (Tweet t in _tweets)
            {
                writer.WriteStartElement("Tweet");
                writer.WriteStartElement("Author");
                writer.WriteString(t.Author.Name.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Title");
                writer.WriteString(t.Title.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("TweetDate");
                writer.WriteString(t.TweetDate.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
            writer.Close();
            MessageBox.Show("kleir");
            }
        }

        private void btnDataBase_Click_1(object sender, RoutedEventArgs e)
        {
            InterSmartV1._1.Menu.DataBase WinDataBase = new Menu.DataBase();
            WinDataBase.Show();
        }

        

        
    }
}
