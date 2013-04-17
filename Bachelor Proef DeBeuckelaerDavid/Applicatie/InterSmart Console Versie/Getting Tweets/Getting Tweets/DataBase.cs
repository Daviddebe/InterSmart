using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getting_Tweets
{
    public class DataBase
    {

        static getTweets tweet = new getTweets();
        static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;


        public void DataProg()
        {
            createNewDatabase();
            connectToDatabase();
            //CreateTable();
            fillTable();
            printUsers();
            //printtweet();
        }


        // Creates an empty database file
        public void createNewDatabase()
        {
            SQLiteConnection.CreateFile("InterSmartDB.sqlite");
        }
        // Creates a connection with our database file.
        public void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }
        //create table
        public void CreateTable()
        {
            string UserTable = "create table User (ID int, name varchar)";
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            command2.ExecuteNonQuery();
        }
        //fill table with info
        public void fillTable()
        {
            //_tweets = tweet.GetCollection();
            //foreach (var t in _tweets)
            //{
            //    string test = t.Author.Name;
            //    string UserTable = "insert into User (ID, name) values (1, $test)";
            //    SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            //    command2.Parameters.AddWithValue("$test", test);
            //    command2.ExecuteNonQuery();

            //}

            string naam = "testeeeeeeeeeee";
            string UserTable = "insert into User (ID, name) values (1, $naam)";
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            command2.Parameters.AddWithValue("$naam", naam);
            command2.ExecuteNonQuery();


        }
        public void printUsers() 
        {
            string UserTable = "select * from User order by ID desc"; //asc of desc hoog-->laag
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            Console.WriteLine("Dit is de database tabel voor de user");
            while (reader2.Read())
                Console.WriteLine("Name: " + reader2["name"] + "\nID: " + reader2["ID"]);
            Console.ReadLine();
        }

        public void printtweet()
        {
            _tweets = tweet.GetCollection();
            if (_tweets.Count() > 0)
            {
                Console.WriteLine("Tweets:");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("---------------------------------------------------");
                foreach (var t in _tweets)
                {
                    Console.WriteLine(t.Author.Name + " tweeted: " + t.Title + " at  " + t.TweetDate);
                    Console.WriteLine("---------------------------------------------------");
                }
                Console.WriteLine("---------------------------------------------------");
            }

        }

    }
}
 