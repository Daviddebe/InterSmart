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
        }
        
    }
}

