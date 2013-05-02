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
        public static DateTime deTijd = new DateTime();
        #endregion

        static void Main(string[] args)
        {
            deTijd = DateTime.Now;

            #region PresentatieContext inladen
            if (args.Length > 0)
            {
                file = args[0];
            }
            Console.WriteLine("Welkom bij InterSmart ! Geopende Presentatie -->" + Path.GetFileNameWithoutExtension(Program.file));
            Console.WriteLine("Geef de antwoorden van de vragen in :");
            Antwoorden = Console.ReadLine();
            Antwoorden.ToCharArray();
            #endregion

            string verder = "";
            verder = Console.ReadLine();
            if (verder == "go")
            {
                DeDataBase.Uitvoeren();
                DeDataBase.VullenDataBase();
                DeDataBase.printUsers();
            }

        //start:
        //    weergeven();
        //    _tweets = tweet.GetCollection();

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
                string title = t.Title;
                title = title.Replace("@Inter_Smart", ""); //@Inter_Smart uit tweets verweideren
                string UpperCasetitle = title.ToUpper(); //Antwoorden in druk letters plaatsen
                string UpperCasetitle2 = title.ToUpper(); //Antwoorden in druk letters plaatsen voor score naar array zetten
                UpperCasetitle2.ToCharArray();
                string nr = UpperCasetitle2[0].ToString();
                char antw = UpperCasetitle2[1];
                int intNr = int.Parse(nr);
                Console.WriteLine(nr + "" + antw);
                if (Program.Antwoorden[intNr -1] == antw) //Score berekenen
                {
                    Console.WriteLine("juist");
                }
                else
                {
                    Console.WriteLine("fout");
                }
            }
        } //tweets weergeven op console

    }
}

