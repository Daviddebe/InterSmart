using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
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
           //CreatePresentationTable();
        //    fillTable();
         //fillTableTweet();
        vreemdesleutels();
         // printUsers();
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
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite; foreign keys = true");
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
            string TweetTable = "create table Tweet (ID INTEGER PRIMARY KEY, Answer varchar UNIQUE, Score int,UserID INTEGER, PresentationID int)";
            SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
            command.ExecuteNonQuery();
        }
        public void CreatePresentationTable()
        {
            string PresentationTable = "create table Presentation (ID INTEGER PRIMARY KEY, PresentationName varchar UNIQUE, QuestionAnswers varchar)";
            SQLiteCommand command3 = new SQLiteCommand(PresentationTable, m_dbConnection);
            command3.ExecuteNonQuery();
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
            {
                Console.WriteLine("Name: " + reader2["name"] + "\nID: " + reader2["ID"]);
            }
            Console.ReadLine();
        }
        public void vreemdesleutels()
        {
            string QuestionAnswers = Program.Antwoorden.ToUpper();
            string presentatie = "";
            string filename = Path.GetFileNameWithoutExtension(Program.file);
            string PresentationName = filename;
            string PresentatieTable = "insert or ignore into Presentation(PresentationName, QuestionAnswers) values ($filename, $QuestionAnswers)";
            SQLiteCommand command10 = new SQLiteCommand(PresentatieTable, m_dbConnection);
            command10.Parameters.AddWithValue("$filename", PresentationName);
            command10.Parameters.AddWithValue("$QuestionAnswers", QuestionAnswers);
            command10.ExecuteNonQuery();

            _tweets = tweet.GetCollection();
            foreach (var t in _tweets)
            {
                string UserId = "";
                string Author = t.Author.Name;
                string title = t.Title;
                title = title.Replace("@Inter_Smart", ""); // @inter_smart er af knippen van de tweets
                string UpperCasetitle = title.ToUpper();// druk letters
                SQLiteParameter param = new SQLiteParameter("@tempString");
                param.Value = Author;
                SQLiteParameter param2 = new SQLiteParameter("@tempString2");
                param2.Value = PresentationName;

                string UserTable = "insert or ignore into User(name) values ($naam)";
                SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
                command2.Parameters.AddWithValue("$naam", Author);
                command2.ExecuteNonQuery();

                string getUserID = "select ID from User where name = \'"+Author.ToString() + "\'";
                SQLiteCommand command5 = new SQLiteCommand(getUserID, m_dbConnection);
                SQLiteDataReader reader = command5.ExecuteReader();
                while (reader.Read())
                    UserId = reader["ID"].ToString();

                string getPresentationID = "select ID from Presentation where PresentationName = \'" + PresentationName + "\'";
                SQLiteCommand command1 = new SQLiteCommand(getPresentationID, m_dbConnection);
                SQLiteDataReader reader2 = command1.ExecuteReader();
                while (reader2.Read())
                    presentatie = reader2["ID"].ToString();

                string TweetTable = "insert or ignore into Tweet(Answer, UserID, PresentationID) values ($title, $UserId, $PresentationID)";
                SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
                command.Parameters.AddWithValue("$title", UpperCasetitle);
                command.Parameters.AddWithValue("$UserId", UserId);
                command.Parameters.AddWithValue("$PresentationID", presentatie);
                command.ExecuteNonQuery();
            }
        }

        public void printtweet()
        {
            string tweetTable = "select * from Tweet";
            SQLiteCommand command = new SQLiteCommand(tweetTable, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            Console.WriteLine("Tweet tabel: ");
            while (reader.Read())
                Console.WriteLine("ID :" + reader["ID"] + "\nAntwoorden:" + reader["Answer"] + "\tUserID :" + reader["UserID"]+ "PresentatieContext: " +reader["PresentationID"]);
            Console.Read();
        }
       
    }
}
 