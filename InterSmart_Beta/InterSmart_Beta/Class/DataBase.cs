using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterSmart_Beta.Class
{
    public class DataBase
    {
        #region Veriable
        static getTweets tweet = new getTweets();
        static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        SQLiteConnection m_dbConnection; //Holds our connection with the database
        #endregion 

        public void Uitvoeren()
        {
            connectToDatabase();
            CreateTable();
        }

        #region Aanmaken DataBase
       
        public void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source = InterSmartDB.sqlite;");
            m_dbConnection.Open();
        }
        public void CreateTable()
        {
            string UserTable = "create table if not exists User (ID INTEGER PRIMARY KEY, name VARCHAR UNIQUE)"; //User Table
            SQLiteCommand command1 = new SQLiteCommand(UserTable, m_dbConnection);
            command1.ExecuteNonQuery();

            string TweetTable = "create table if not exists Tweet (ID INTEGER PRIMARY KEY, Answer varchar, Score int,UserID INTEGER, PresentationID int)"; //Tweet Table
            SQLiteCommand command2 = new SQLiteCommand(TweetTable, m_dbConnection);
            command2.ExecuteNonQuery();

            string PresentationTable = "create table if not exists Presentation (ID INTEGER PRIMARY KEY, PresentationName varchar UNIQUE, QuestionAnswers varchar)";//Presentation Table
            SQLiteCommand command3 = new SQLiteCommand(PresentationTable, m_dbConnection);
            command3.ExecuteNonQuery();

            string ScoreTable = "create table if not exists Score(ID INTEGER PRIMARY KEY, PresentationID INTEGER, UserID INTEGER, Score INTEGER)"; //Score Table
            SQLiteCommand command4 = new SQLiteCommand(ScoreTable, m_dbConnection);
            command4.ExecuteNonQuery();
        }
        #endregion

        #region Vullen van de Database
        public void VullenDataBase()
        {
            #region Vullen PresentatieTable

            string PresentationName = Path.GetFileNameWithoutExtension(Program.file);
            string QuestionAnswers = Program.Antwoorden.ToUpper();
            string PresentatieTable = "insert or ignore into Presentation(PresentationName, QuestionAnswers) values ($filename, $QuestionAnswers)";
            SQLiteCommand command1 = new SQLiteCommand(PresentatieTable, m_dbConnection);
            command1.Parameters.AddWithValue("$filename", PresentationName);
            command1.Parameters.AddWithValue("$QuestionAnswers", QuestionAnswers);
            command1.ExecuteNonQuery();

            #endregion

            _tweets = tweet.GetCollection();
            System.Threading.Thread.Sleep(3000);//3sec w8e anders zaagt em da de collectie veranderd is
            ObservableCollection<Tweet> _tweetsTijd = new ObservableCollection<Tweet>();

            foreach (var t in _tweets)
            {
                DateTime TweetTijd = new DateTime();
                TweetTijd = Convert.ToDateTime(t.TweetDate);
                if (TweetTijd > Program.deTijd)
                {
                    #region Variable
                    string UserID = "";
                    string PresentationID = "";
                    string Author = t.Author.Name;
                    string title = t.Title;
                    title = title.Replace("@Inter_Smart", ""); //@Inter_Smart uit tweets verweideren
                    string UpperCasetitle = title.ToUpper(); //Antwoorden in druk letters plaatsen
                    string UpperCasetitle2 = title.ToUpper(); //Antwoorden in druk letters plaatsen voor score naar array zetten
                    UpperCasetitle2.ToCharArray();
                    SQLiteParameter param = new SQLiteParameter("@tempString");
                    param.Value = Author;
                    SQLiteParameter param2 = new SQLiteParameter("@tempString2");
                    param2.Value = PresentationName;
                    string nr = UpperCasetitle2[0].ToString();
                    char antw = UpperCasetitle2[1];
                    int intNr = int.Parse(nr);
                    #endregion
                
                    
                    #region Users Toevoegen
                    string UserTable = "insert or ignore into User(name) values ($naam)";
                    SQLiteCommand command2 = new SQLiteCommand(UserTable, m_dbConnection);
                    command2.Parameters.AddWithValue("$naam", Author);
                    command2.ExecuteNonQuery();
                    #endregion

                    #region UserID van users halen

                    string getUserID = "select ID from User where name = \'" + Author.ToString() + "\'";
                    SQLiteCommand command3 = new SQLiteCommand(getUserID, m_dbConnection);
                    SQLiteDataReader reader = command3.ExecuteReader();
                    while (reader.Read())
                        UserID = reader["ID"].ToString();

                    #endregion

                    #region PresentationID van presentaties halen

                    string getPresentationID = "select ID from Presentation where PresentationName = \'" + PresentationName + "\'";
                    SQLiteCommand command4 = new SQLiteCommand(getPresentationID, m_dbConnection);
                    SQLiteDataReader reader2 = command4.ExecuteReader();
                    while (reader2.Read())
                        PresentationID = reader2["ID"].ToString();

                    #endregion

                    #region Tweets opslaan in database met UserID en PresentationID en score

                    if (Program.Antwoorden[intNr-1] == antw) //Score berekenen
                    {
                        string TweetTable = "insert into Tweet(Answer, UserID, PresentationID, Score) values ($title, $UserId, $PresentationID, 1)";
                        SQLiteCommand command5 = new SQLiteCommand(TweetTable, m_dbConnection);
                        command5.Parameters.AddWithValue("$title", UpperCasetitle);
                        command5.Parameters.AddWithValue("$UserId", UserID);
                        command5.Parameters.AddWithValue("$PresentationID", PresentationID);
                        command5.ExecuteNonQuery();
                    }
                    else
                    {
                        string TweetTable = "insert into Tweet(Answer, UserID, PresentationID, Score) values ($title, $UserId, $PresentationID, 0)";
                        SQLiteCommand command5 = new SQLiteCommand(TweetTable, m_dbConnection);
                        command5.Parameters.AddWithValue("$title", UpperCasetitle);
                        command5.Parameters.AddWithValue("$UserId", UserID);
                        command5.Parameters.AddWithValue("$PresentationID", PresentationID);
                        command5.ExecuteNonQuery();
                        Console.WriteLine(antw);
                        Console.ReadLine(); 
                    }
                   
                    #endregion 
                    
                   
                }
               
            }
        }
        #endregion

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

       
    }
}
