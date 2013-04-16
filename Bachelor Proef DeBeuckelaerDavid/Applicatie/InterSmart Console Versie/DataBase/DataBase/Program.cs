using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace DataBase
{
    class Program
    {
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;

        static void Main(string[] args)
        {
            Program p = new Program();
        }

        public Program()
        {
            createNewDatabase();
            connectToDatabase();
            //CreateTable();
           // CreateTable2();
            //fillTable();
            //fillTable2();
            //printUsers();
            //printtweets();
         
        }
        // Creates an empty database file
        void createNewDatabase()
        {
            SQLiteConnection.CreateFile("InterSmartDB.sqlite");
        }

        // Creates a connection with our database file.
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }
        //create table
        void CreateTable()
        {
            string UserTable = "create table User (ID int, name varchar)";
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            command2.ExecuteNonQuery();
        }
        void CreateTable2()
        {
            string TweetTable = "create table Tweet (ID int, UserID int, PresentationID int, Answer varchar, Score int, PresentationContext varchar, QuestionAnswers varchar)";
            SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // Inserts some values in the tweet table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        void fillTable()
        {
            string UserTable = "insert into User (ID, name) values (1, 'David')";
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            command2.ExecuteNonQuery();
        }
        void fillTable2()
        {
            string TweetTable = "insert into Tweet (ID, UserID, PresentationID, Answer, Score, PresentationContext, QuestionAnswers) values (0,0,0,'Antwoord = Test', 0, 'PresentatieTest','aabbccddaa')";
            SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
            command.ExecuteNonQuery();
        }


        // Writes the tweet to the console sorted on score in descending order.
       
        void printUsers()
        {
            string UserTable = "select * from User order by ID desc"; //asc of desc hoog-->laag
            SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            Console.WriteLine("Dit is de database tabel voor de user");
            while (reader2.Read())
                Console.WriteLine("Name: " + reader2["name"] + "\nID: " + reader2["ID"]);
            Console.ReadLine();
        }

        void printtweets()
        {
            string TweetTable = "select * from Tweet order by ID desc";
            SQLiteCommand command = new SQLiteCommand(TweetTable, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            Console.WriteLine("Dit is de database table voor Tweets :");
            while (reader.Read())
            {
                Console.WriteLine("ID: " + reader["ID"] + "\nAnswer: " + reader["Answer"]);
                Console.WriteLine("\nPresentationID: " + reader["PresentationID"]);
                Console.WriteLine("\nAnswer: " + reader["Answer"] + "\nScore: " + reader["Score"]);
                Console.WriteLine("\nPresentationContext: " + reader["PresentationContext"] + "\nQuestionAnswers :" + reader["QuestionAnswers"]);
                Console.ReadLine();
            }
        }
    }
        
    }

