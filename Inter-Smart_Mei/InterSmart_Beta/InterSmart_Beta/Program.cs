using InterSmart_Beta.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterSmart_Beta
{
    class Program
    {
        #region Variable
        static getTweets tweet = new getTweets();
        static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        static DataBase DeDataBase = new DataBase();
        public static string file;
        public static string Antwoorden;
        public static DateTime deTijd = DateTime.Now;
        public static int intVerder;
         //DateTime.Now;
        //new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 22, 19, 00);
        #endregion

        static void Main(string[] args)
        {

            #region internetconnectie testen
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie nakijken
            {
                Console.WriteLine("Er is geen internet, kijk u connectie na !");
                Console.WriteLine("(Druk ENTER om de applicatie af te sluiten)");
                Console.ReadLine();
                Environment.Exit(0);
            }
            #endregion

            #region PresentatieContext inladen
            if (args.Length > 0)
            {
                file = args[0];
            }
            Console.WriteLine("Welkom bij InterSmart ! Geopende Presentatie -->" + Path.GetFileNameWithoutExtension(Program.file));
            Console.WriteLine("Geef de antwoorden van de vragen in :");
            string iets = Console.ReadLine();
            Antwoorden = iets.ToUpper();
            Antwoorden.ToCharArray();
            #endregion

            #region Internetconnectie nakijken
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie nakijken
            {
                Console.WriteLine("Geen verbinding met het internet");
                Console.WriteLine("Kijk u connectie na, als deze hersteld is drukt u op \"ENTER\"");
                Console.ReadLine();
                while (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie terug 
                {
                    Console.WriteLine("Nog steeds geen connectie, probeer nogmaals");
                    Console.ReadLine();
                }
                Console.WriteLine("Verbinding is in orde, de tweets worden nu binnengehaalt");
            }
            #endregion

            Console.WriteLine("Geef nummer in van de vraag waar u tussenresultaten van wild bekijken");
            intVerder = int.Parse(Console.ReadLine());

            if (intVerder > 0)
            {
                DeDataBase.VraagStatistieken();
            }
            Console.WriteLine("aangemaakt: type go en druk enter voor eindresultaten");
            string verder = "";
            verder = Console.ReadLine();
            if (verder == "go") //totaal eindresultaat
            {
                DeDataBase.Uitvoeren();
                DeDataBase.VullenDataBase();
                DeDataBase.OomfoCharts();
                Console.WriteLine("Scores zijn succesvol ingeladen ! (druk ENTER voor statistieken in te laden)");
                Console.ReadLine();
                DeDataBase.StatistiekenInDBOpslaan();
                Console.WriteLine("Statistieken inladen is een succes !");
                Console.ReadLine();
                // DeDataBase.printUsers();
            }

        //start:
        //    _tweets = tweet.show();
        //    weergeven();
        //    if (_tweets.Count() <= 0)
        //    {
        //        goto start;
        //    }
        //    else
        //    {
        //        //DataProg.print();
        //        //DataProg.OomfoChart();
        //    }

        //    Console.WriteLine("Press \'q\' to quit the sample.");
        //    while (Console.Read() != 'q') ;

        }

        public static void weergeven()
        {
            Console.WriteLine("De tweets");
            System.Threading.Thread.Sleep(3000);
            foreach (var t in _tweets)
            {
              //  Console.WriteLine(t.Title + _tweets.Count.ToString());
            }
        } //tweets weergeven op console

    }
}

