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
using System.Xml.Linq;

namespace InterSmartV1._1.Menu
{
    /// <summary>
    /// Interaction logic for Statistieken.xaml
    /// </summary>
    public partial class Statistieken : Window
    {
        public static getTweets tweet;
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        public List<int> AnswerList = new List<int>();
       
       
        public Statistieken()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Statistieken_Loaded);
        }

        private void Statistieken_Loaded(object sender, RoutedEventArgs e)
        {
            //lstbxStats.ItemsSource = _tweets;
        }
       

        private void BtnShowStats_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //simpele berekening voor % van een antwoord.
                AnswerList = MainPage.tweet.GetAnswers();
                double totaal = AnswerList[4];
                double berekeningA = (AnswerList[0] / totaal * 100);
                double berekeningB = (AnswerList[1] / totaal * 100);
                double berekeningC = (AnswerList[2] / totaal * 100);
                double berekeningD = (AnswerList[3] / totaal * 100);
                lstbxStats.Items.Clear();
                lstbxStats.Items.Add("A = " + (AnswerList[0]) + " --> "+ Math.Round(berekeningA, 2) + "%"); //math round is voor 2 cijfers na de komma
                lstbxStats.Items.Add("B = " + (AnswerList[1]) + " --> "+ Math.Round(berekeningB, 2) + "%");
                lstbxStats.Items.Add("C = " + (AnswerList[2]) + " --> "+ Math.Round(berekeningC, 2) + "%");
                lstbxStats.Items.Add("D = " + (AnswerList[3]) + " --> "+ Math.Round(berekeningD, 2) + "%");
                lstbxStats.Items.Add("Totaal = " + AnswerList[4]);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AnswerList.Clear();
            this.Hide();
            
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Kies een bestaand txt bestand of maak een nieuwe aan");
                OpenFileDialog f = new OpenFileDialog();
                f.ShowDialog();
                string textout = "";
                string test = Environment.NewLine;
                foreach (string li in lstbxStats.Items)
                {
                    textout = textout + li + Environment.NewLine;
                }
                textout = "Statistieken" + Environment.NewLine + textout + "";
                File.WriteAllText(f.FileName, textout);
                MessageBox.Show("Load Succesvol");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
 
        }

    }
}
