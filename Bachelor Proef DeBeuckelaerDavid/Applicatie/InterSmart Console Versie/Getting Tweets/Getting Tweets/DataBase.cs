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
            //CreateTabelScore();
        //    fillTable();
         //fillTableTweet();
        vreemdesleutels();
          //printUsers();
           //printtweet();
            print();
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

        public void CreateTabelScore()
        {
            string ScoreTable = "create table Score(ID INTEGER PRIMARY KEY, PresentationID INTEGER, UserID INTEGER, Score INTEGER)";
            SQLiteCommand command30 = new SQLiteCommand(ScoreTable, m_dbConnection);
            command30.ExecuteNonQuery();
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
            string tweetTable = "select * from Tweet ";
            SQLiteCommand command = new SQLiteCommand(tweetTable, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            Console.WriteLine("Tweet tabel: ");
            while (reader.Read())
                Console.WriteLine("\nAntwoorden:" + reader["Answer"] + "\tUserID :" + reader["UserID"]+ "PresentatieContext: " +reader["PresentationID"]);
            Console.Read();
        }

        public void Score()
        {
            _tweets = tweet.GetCollection();
            //Program.Antwoorden[0].ToString(); // eerste antwoord
            foreach (var t in _tweets)
            {
                string Author = t.Author.Name;
                //where presentatieID is momentel presentatie ID --> viarable presentatie in methode vreemdesleutel
                
            }
        }
        public void print()
        {
            _tweets = tweet.GetCollection();
            string presentatie = "";
            HashSet<int> lijstTweets = new HashSet<int>();
            List<String> UserTweets = new List<string>();
            string Antwoord1 = Program.Antwoorden[0].ToString();
            string Antwoord2 = Program.Antwoorden[1].ToString();
            string Antwoord3 = Program.Antwoorden[2].ToString();
            string Antwoord4 = Program.Antwoorden[3].ToString();
            string Antwoord5 = Program.Antwoorden[4].ToString();
            int scoren = 0;

            string filename = Path.GetFileNameWithoutExtension(Program.file);
            string PresentationName = filename;
            string getPresentationID = "select ID from Presentation where PresentationName = \'" + PresentationName + "\'";
            SQLiteCommand command1 = new SQLiteCommand(getPresentationID, m_dbConnection);
            SQLiteDataReader reader2 = command1.ExecuteReader();
            while (reader2.Read())
            {
                presentatie = (reader2["ID"]).ToString(); //momentele ID van presentatie er uit halen
            }
            
                string tweetTable = "select UserID from Tweet where PresentationID = $Presentatie";
                SQLiteCommand command15 = new SQLiteCommand(tweetTable, m_dbConnection);
                command15.Parameters.AddWithValue("$Presentatie", presentatie);
                SQLiteDataReader reader15 = command15.ExecuteReader();
                while (reader15.Read())
                {
                    lijstTweets.Add(reader15.GetInt32(0));  //lijst met alle users van de momenteel geopende ID
                }

                foreach (var item in lijstTweets)
                {
                    Console.WriteLine(item);
                    string tweetUsers = "select Answer, UserID from Tweet where UserID = \'" + item + "\' AND PresentationID = \'" + presentatie + "\'";
                    SQLiteCommand commandUser = new SQLiteCommand(tweetUsers, m_dbConnection);
                    SQLiteDataReader readerUser = commandUser.ExecuteReader();
                    while (readerUser.Read())
                        UserTweets.Add(readerUser.GetString(0));
                    UserTweets.ForEach(Console.WriteLine);
                    foreach (var antwoord in UserTweets)
                    {

                        if (antwoord.Contains("1")) //vraag 1
                        {
                            if (antwoord.Contains(Antwoord1.ToUpper()))
                            {
                                scoren ++;
                                Console.WriteLine("Juist" + antwoord.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Fout");
                            }
                        }

                        else if (antwoord.Contains("2")) //vraag 1
                        {
                            if (antwoord.Contains(Antwoord2.ToUpper()))
                            {
                                scoren ++;
                                Console.WriteLine("Juist" + antwoord.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Fout");
                            }
                        }
                        else if (antwoord.Contains("3")) //vraag 1
                        {
                            if (antwoord.Contains(Antwoord3.ToUpper()))
                            {
                                scoren ++;
                                Console.WriteLine("Juist" + antwoord.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Fout");
                            }
                        }
                        else if (antwoord.Contains("4")) //vraag 1
                        {
                            if (antwoord.Contains(Antwoord4.ToUpper()))
                            {
                                scoren ++;
                                Console.WriteLine("Juist" + antwoord.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Fout");
                            }
                        }
                        else if (antwoord.Contains("5")) //vraag 1
                        {
                            if (antwoord.Contains(Antwoord5.ToUpper()))
                            {
                                scoren ++;
                                Console.WriteLine("Juist" + antwoord.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Fout");
                            }
                        }
                    }
                    Console.WriteLine(scoren.ToString());
                    string insertscore = "insert or ignore into Score(UserID, PresentationID, Score) values($UserID, $PresentationID, $Score)";
                    SQLiteCommand command33 = new SQLiteCommand(insertscore, m_dbConnection);
                    command33.Parameters.AddWithValue("$UserID",item);
                    command33.Parameters.AddWithValue("$PresentationID",presentatie);
                    command33.Parameters.AddWithValue("$Score",scoren);
                    command33.ExecuteNonQuery();
                    Console.ReadLine();
                    scoren = 0;
                    UserTweets.Clear();
                }

                Console.Read();

        }
        
    }
}
 