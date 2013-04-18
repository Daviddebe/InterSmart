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
            //CreateTweetTable();
            //fillTable();
            //fillTableTweet();
            //printUsers();
             printtweet();
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
            string UserTable = "create table User (ID INTEGER PRIMARY KEY, name VARCHAR UNIQUE)";
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            command2.ExecuteNonQuery();
        }
        public void CreateTweetTable()
        {
            string TweetTable = "create table Tweet (ID INTEGER PRIMARY KEY,UserID INTEGER,  Answer varchar UNIQUE, Score int, FOREIGN KEY(UserID) REFERENCES User(ID))";
            SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
            command.ExecuteNonQuery();
        }
        //fill table with info
        public void fillTable()
        {
            _tweets = tweet.GetCollection();
            foreach (var t in _tweets)
            {
            string naam = t.Author.Name;
            string UserTable = "insert or ignore into User(name) values ($naam)";
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            command2.Parameters.AddWithValue("$naam", naam);
            command2.ExecuteNonQuery();
            }
           

        }

        public void fillTableTweet()
        {
            _tweets = tweet.GetCollection();
            foreach (var t in _tweets)
            {
                string title = t.Title;
                string TweetTable = "insert or ignore into Tweet(Answer) values ($title)";
                SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
                command.Parameters.AddWithValue("$title", title);
                command.ExecuteNonQuery();
            }


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
            string tweetTable = "select * from Tweet order by Answer desc";
            SQLiteCommand command = new SQLiteCommand(tweetTable, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            Console.WriteLine("Tweet tabel: ");
            while (reader.Read())
                Console.WriteLine("Antwoorden:" + reader["Answer"]);
            Console.Read();

        }

    }
}
 