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
        static DataBase DeDataBase = new DataBase();
        public static string file;
        public static string Antwoorden;
        public static DateTime deTijd = DateTime.Now;
        public static int intVerder;
        static getTweets tweet = new getTweets();
        public static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
      
         //DateTime.Now;
        //new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 22, 19, 00);
        #endregion

        static void Main(string[] args)
        {
           
            #region internetconnectie testen
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie nakijken
            {
                Console.WriteLine("There is no internet, check your connection.");
                Console.WriteLine("(Press ENTER to exit.)");
                Console.ReadLine();
                Environment.Exit(0);
            }
            #endregion

            #region PresentatieContext inladen
            if (args.Length > 0)
            {
                file = args[0];
            }
            Console.WriteLine("Welcome to InterSmart ! Current presentation -->" + Path.GetFileNameWithoutExtension(Program.file));
            Console.WriteLine("Insert the correct answers of the questions:");
            string iets = Console.ReadLine();
            Antwoorden = iets.ToUpper();
            Antwoorden.ToCharArray();
            #endregion

            #region Internetconnectie nakijken
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie nakijken
            {
                Console.WriteLine("No Internet connection!");
                Console.WriteLine("Check your connection, and if it is repaired press \"ENTER\"");
                Console.ReadLine();
                while (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie terug 
                {
                    Console.WriteLine("Still no connection, try again");
                    Console.ReadLine();
                }
                Console.WriteLine("Connection is back, system will continue progress. ");
            }
            #endregion
          
            Console.WriteLine("The quiz has started!");
            Console.WriteLine("1. Show statistics for a single question");
            Console.WriteLine("2. Show total score and statistics");
            Console.WriteLine("3. Quit");
            int verder = 99;
            while (verder != 0)
            {
                verder = int.Parse(Console.ReadLine());
                switch (verder)
                {
                    case 1:

                        #region Internetconnectie nakijken
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie nakijken
                        {
                            Console.WriteLine("No Internet connection!");
                            Console.WriteLine("Check your connection, and if it is repaired press \"ENTER\"");
                            Console.ReadLine();
                            while (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie terug 
                            {
                                Console.WriteLine("Still no connection, try again");
                                Console.ReadLine();
                            }
                            Console.WriteLine("Connection is back, system will continue progress. ");
                        }
                        #endregion

                        Console.WriteLine("Input question number:");
                        intVerder = int.Parse(Console.ReadLine());
                        Console.WriteLine("Please wait...");
                        DeDataBase.VraagStatistieken();
                        DeDataBase.tweet._tweets.Clear();
                        Console.WriteLine("Statistics have been made");
                        Console.WriteLine("1. Show statistics for a single question");
                        Console.WriteLine("2. Show total score and statistics");
                        Console.WriteLine("3. Quit");
                        break;

                    case 2:

                        #region Internetconnectie nakijken
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie nakijken
                        {
                            Console.WriteLine("No Internet connection!");
                            Console.WriteLine("Check your connection, and if it is repaired press \"ENTER\"");
                            Console.ReadLine();
                            while (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString() == "False") //internet connectie terug 
                            {
                                Console.WriteLine("Still no connection, try again");
                                Console.ReadLine();
                            }
                            Console.WriteLine("Connection is back, system will continue progress. ");
                        }
                        #endregion

                        Console.WriteLine("Please wait...");
                        DeDataBase.Uitvoeren();
                        DeDataBase.VullenDataBase();
                        DeDataBase.OomfoCharts();
                        Console.WriteLine("Total score completed!");
                        DeDataBase.StatistiekenInDBOpslaan();
                        Console.WriteLine("Overview statistics are completed!");
                        Console.WriteLine("Presentation has ended, press ENTER to close InterSmart.");
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;

                    case 3:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Sorry, invalid selection");
                        break;
                }
            }
        }
    }
}

