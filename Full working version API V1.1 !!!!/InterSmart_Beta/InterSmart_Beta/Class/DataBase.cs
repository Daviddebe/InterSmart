﻿using System;
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
        public getTweets tweet = new getTweets();
        static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        SQLiteConnection m_dbConnection; //Holds our connection with the database
        public List<double> procenten = new List<double>();
        private List<string> resultatenLijst = new List<string>();
        public List<int> AnswerList = new List<int>();
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

            string ScoreTable = "create table if not exists Score(ID INTEGER PRIMARY KEY, PresentationID INTEGER, Vraag VARCHAR, Score INTEGER)"; //Score Table
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
               int uur = t.TweetDate.Hour;
               DateTime tweetijd = new DateTime();
               TimeSpan time = new TimeSpan(uur + 2, t.TweetDate.Minute, t.TweetDate.Second);
               tweetijd = new DateTime(t.TweetDate.Year, t.TweetDate.Month, t.TweetDate.Day, uur + 2, t.TweetDate.Minute, t.TweetDate.Second);  
                if (tweetijd> Program.deTijd)
                {
                    //System.Console.WriteLine(tweetijd.ToString() + " > nu: " + Program.deTijd.ToString());

                    #region Variable
                    bool check;
                    string UserID = "";
                    string PresentationID = "";
                    string Author = t.Author.Name;
                    string title = t.Title;
                    int unicode = 255;
                    char enter = (char)unicode;
                    string strEnter = enter.ToString();
                    title = title.Replace("@Inter_Smart", ""); //@Inter_Smart uit tweets verwijderen
                    title = title.Replace(" ", "");
                    title = title.Replace("\n", "");
                    //title = title.Replace(strEnter, "");
                    string UpperCasetitle = title.ToUpper(); //Antwoorden in druk letters plaatsen
                    string UpperCasetitle2 = title.ToUpper(); //Antwoorden in druk letters plaatsen voor score naar array zetten
                    UpperCasetitle2.ToCharArray();
                    SQLiteParameter param = new SQLiteParameter("@tempString");
                    param.Value = Author;
                    SQLiteParameter param2 = new SQLiteParameter("@tempString2");
                    param2.Value = PresentationName;
                    string nr = "";
                    int intNr = 1;
                    char antw = 'z';
                    try
                    {
                        nr = UpperCasetitle2[0].ToString();
                        intNr = int.Parse(nr);
                        antw = UpperCasetitle2[1];
                    }
                    catch (Exception)
                    {
                        intNr = 999;
                        antw = 'z';
                       // System.Console.WriteLine("typt da is juist !");
                    }
                    
                    #endregion

                    t.vraagNr = intNr;
                
                    
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
                    int count = UpperCasetitle2.Count();
                    switch (count)
                    {
                        case 2:
                            check = checklength(Program.Antwoorden.Count(), intNr);
                           
                            if (check == true)
                            {
                                if (Program.Antwoorden[intNr - 1] == antw) //Score berekenen
                                {
                                    t.EvaluatieVraag = 1;
                                    string TweetTable = "insert into Tweet(Answer, UserID, PresentationID, Score) values ($title, $UserId, $PresentationID, 1)";
                                    SQLiteCommand command5 = new SQLiteCommand(TweetTable, m_dbConnection);
                                    command5.Parameters.AddWithValue("$title", UpperCasetitle);
                                    command5.Parameters.AddWithValue("$UserId", UserID);
                                    command5.Parameters.AddWithValue("$PresentationID", PresentationID);
                                    command5.ExecuteNonQuery();
                                }
                                else
                                {
                                    t.EvaluatieVraag = 0;
                                    string TweetTable = "insert into Tweet(Answer, UserID, PresentationID, Score) values ($title, $UserId, $PresentationID, 0)";
                                    SQLiteCommand command5 = new SQLiteCommand(TweetTable, m_dbConnection);
                                    command5.Parameters.AddWithValue("$title", UpperCasetitle);
                                    command5.Parameters.AddWithValue("$UserId", UserID);
                                    command5.Parameters.AddWithValue("$PresentationID", PresentationID);
                                    command5.ExecuteNonQuery();
                                }
                            }

                            break;
                        case 3:
                            string nr2 = UpperCasetitle2[1].ToString();
                            antw = UpperCasetitle2[2];
                            string nummer = nr + nr2;
                            int nummerint = 1;
                            int tussewaarde = 0;
                            try
                            {
                                tussewaarde = Convert.ToInt32(nummer);
                            }
                            catch 
                            {
                                
                            }
                            t.vraagNr = tussewaarde;
                            try
                            {
                                nummerint = int.Parse(nummer);
                                //System.Console.WriteLine(nummerint);
                                //System.Console.ReadLine();
                            }
                            catch (Exception)

                            {
                                //System.Console.WriteLine(nummer);
                                //System.Console.ReadLine();
                            }
                            check = checklength(Program.Antwoorden.Count(), nummerint);
                            if (check == true)
                            {
                                //System.Console.WriteLine(Program.Antwoorden.Count().ToString() + "de " + nummerint + "de vraag.");
                                //System.Console.ReadLine();
                                if (Program.Antwoorden[nummerint - 1] == antw) //Score berekenen
                                {
                                    t.EvaluatieVraag = 1;
                                    string TweetTable = "insert into Tweet(Answer, UserID, PresentationID, Score) values ($title, $UserId, $PresentationID, 1)";
                                    SQLiteCommand command5 = new SQLiteCommand(TweetTable, m_dbConnection);
                                    command5.Parameters.AddWithValue("$title", UpperCasetitle);
                                    command5.Parameters.AddWithValue("$UserId", UserID);
                                    command5.Parameters.AddWithValue("$PresentationID", PresentationID);
                                    command5.ExecuteNonQuery();
                                }
                                else
                                {
                                    t.EvaluatieVraag = 0;
                                    string TweetTable = "insert into Tweet(Answer, UserID, PresentationID, Score) values ($title, $UserId, $PresentationID, 0)";
                                    SQLiteCommand command5 = new SQLiteCommand(TweetTable, m_dbConnection);
                                    command5.Parameters.AddWithValue("$title", UpperCasetitle);
                                    command5.Parameters.AddWithValue("$UserId", UserID);
                                    command5.Parameters.AddWithValue("$PresentationID", PresentationID);
                                    command5.ExecuteNonQuery();
                                }
                            }
                            break;
                        default:
                          //  System.Console.WriteLine("een van de antwoorden voldoet niet aan de standaard --> " + t.Author.Name.ToString());
                            break;
                    }
                    #endregion

                }
             
            }
        }
        #endregion

     
        public void OomfoCharts()
        {
            #region Variable
            _tweets = tweet.GetCollection();
            string presentatieID = "";
            string filename = Path.GetFileNameWithoutExtension(Program.file);
            string PresentationName = filename;
            HashSet<int> UserIDLijst = new HashSet<int>();
            List<string> OomfoScores = new List<string>();
            HashSet<string> OomfoNamen = new HashSet<string>();
            int totaalscore = 0;
            List<String> dataset = new List<string>();
            List<String> dataset2 = new List<string>();
            System.IO.StreamWriter file = new System.IO.StreamWriter("TotalScore.txt");
            #endregion 

            #region ID van momenteel geopende presentatie nemen

            string getPresentationID = "select ID from Presentation where PresentationName = \'" + PresentationName + "\'";
            SQLiteCommand command = new SQLiteCommand(getPresentationID, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                presentatieID = (reader["ID"]).ToString(); //momentele ID van presentatie er uit halen
            }

            #endregion 

            #region Users selecteren en in lijst plaatsen
            string selectUsers = "select UserID from tweet where PresentationID =\'" + presentatieID + "\'";
                SQLiteCommand command2 = new SQLiteCommand(selectUsers, m_dbConnection);
                SQLiteDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    UserIDLijst.Add(reader2.GetInt32(0));
                }

                foreach (var item in UserIDLijst)
                {
                    string geetUsers = "select name from user where ID = '" + item + "\'";
                    SQLiteCommand command4 = new SQLiteCommand(geetUsers, m_dbConnection);
                    SQLiteDataReader reader4 = command4.ExecuteReader();
                    while (reader4.Read())
                    {
                        OomfoNamen.Add(reader4["name"].ToString());
                    }
                    foreach (var naam in OomfoNamen)
                    {
                        
                        dataset2.Add(Environment.NewLine + "<category label=" + "\"" + naam + "\"" + "/>");
                        
                    }
                    OomfoNamen.Clear();
            #endregion

            #region score brekenen
                    string getscores = "select score from tweet where UserID = \'" + item + "\' and PresentationID = \'" + presentatieID + "\'";
                    SQLiteCommand command3 = new SQLiteCommand(getscores, m_dbConnection);
                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        OomfoScores.Add(reader3["score"].ToString());
                    }
                    foreach (var item2 in OomfoScores)
                    {
                        if (item2.Contains("1"))
                        {
                            totaalscore += 1;
                        }
                        
                    }
                    dataset.Add(Environment.NewLine + "<set value=" + "\"" + totaalscore+ "\"" + "/>");
                    //file.WriteLine(totaalscore.ToString());
                    OomfoScores.Clear();
                    totaalscore = 0;
                }
                    #endregion 

            #region Naar oomfofile schrijven
                string textout = "";
                textout = "<?xml version=" + "\"1.0\"" + " encoding=" + "\"UTF-16\"" + " standalone=" + "\"yes\"" + "?>"
                    + Environment.NewLine + "<chart caption=" + "\"Score\"" + " subcaption=" + "\"" + filename + "\"" + " animation=" + "\"1\"" + " xaxisname=" + "\"Deelnemers\"" + " yaxisname=" + "\"Aantal\"" + ">"
                    + Environment.NewLine + "<categories>";

                 string textout1= Environment.NewLine + "</categories>"
                    + Environment.NewLine + "<dataset seriesName=" + "\""+Program.Antwoorden.Length.ToString()+"\"" + ">";
                string textout2 = "";
                textout2 = Environment.NewLine + "</dataset>"
                    + Environment.NewLine + "<PP_Internal>"
                    + Environment.NewLine + "<chart animation=" + "\"1\"" + " themename=" + "\"Custom\"" + "/>"
                    + Environment.NewLine + "</PP_Internal>"
                    + Environment.NewLine + "</chart>";
                file.WriteLine(textout);
                dataset2.ForEach(file.WriteLine);
                file.WriteLine(textout1);
                dataset.ForEach(file.WriteLine);
                file.WriteLine(textout2);
                file.Close();
                #endregion 
        }

        public void StatistiekenInDBOpslaan() //totaal statistieken voor eind resultaat
        {
            string filename = Path.GetFileNameWithoutExtension(Program.file);
            double totaal = 0;
            double juist = 0;
            double procent = 0;
            int antwoorden = Program.Antwoorden.Count() +1;// +1 omdat het array is
            _tweets = tweet.GetCollection();
            List<string> lijstProcenten = new List<string>();
            List<string> lijstVragen = new List<string>();
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("OverviewQuestionsStats.txt");

                for (int i = 1; i < antwoorden; i++)
                {
                    foreach (var t in _tweets)
                    {
                        if (t.vraagNr == i)
                        {
                            if (t.EvaluatieVraag == 1)
                            {
                                juist++;
                            }
                            else if (t.EvaluatieVraag == 0)
                            {
                                //nix eig
                            }
                            totaal++;
                        }
                    }
                    procent =((juist / totaal)*100);
                    procent = Math.Round(procent);
                    procenten.Add(procent);
                    totaal = 0;
                    juist = 0;
                }

                #region uitschrijven naar xml 
                for (int i = 0; i < procenten.Count; i++)
                {
                    //System.Console.WriteLine("vraag" + (i + 1).ToString() + ": " + procenten[i].ToString() + "% juist");
                    lijstProcenten.Add(Environment.NewLine + "<set value=" + "\"" + procenten[i].ToString() + "\"" + "/>");
                    lijstVragen.Add(Environment.NewLine + "<category label=" + "\"" + "Vraag" + (i + 1).ToString() + "\"" + "/>");
                 
                }
                string textout = "";
                textout = "<?xml version=" + "\"1.0\"" + " encoding=" + "\"UTF-16\"" + " standalone=" + "\"yes\"" + "?>"
                    + Environment.NewLine + "<chart caption=" + "\"Statistieken\"" + " subcaption=" + "\"" + filename + "\"" + " animation=" + "\"1\"" + " xaxisname=" + "\"Gestelde vragen\"" + " yaxisname=" + "\"Aantal\"" + ">"
                    + Environment.NewLine + "<categories>";

                string textout1 = Environment.NewLine + "</categories>"
                   + Environment.NewLine + "<dataset seriesName=" + "\"Aantal % juist geantwoord\"" + ">";
                string textout2 = "";
                textout2 = Environment.NewLine + "</dataset>"
                    + Environment.NewLine + "<PP_Internal>"
                    + Environment.NewLine + "<chart animation=" + "\"1\"" + " themename=" + "\"Custom\"" + "/>"
                    + Environment.NewLine + "</PP_Internal>"
                    + Environment.NewLine + "</chart>";
                file2.WriteLine(textout);
                lijstVragen.ForEach(file2.WriteLine);
                file2.WriteLine(textout1);
                lijstProcenten.ForEach(file2.WriteLine);
                file2.WriteLine(textout2);
                file2.Close();
                #endregion 
        }

        public bool checklength(int vragen, int antwoord)
        {
            if (vragen < antwoord)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void VraagStatistieken()
        {
            #region Variable 
            _tweets = tweet.GetCollection();
            int deVraag = Program.intVerder;
            string xmlfile = "Question"+ deVraag.ToString()+ ".txt";
            System.IO.StreamWriter file3 = new System.IO.StreamWriter(xmlfile);
            int A = 0;
            int B = 0;
            int C = 0;
            int D = 0;
            int totaal = 0;
            int antwoorden = Program.Antwoorden.Count() + 1;// +1 omdat het array is
            System.Threading.Thread.Sleep(3000);//3sec w8e anders zaagt em da de collectie veranderd is
            string filename = Path.GetFileNameWithoutExtension(Program.file);
            ObservableCollection<Tweet> _tweetsTijd = new ObservableCollection<Tweet>();
            #endregion 

            #region berekenen statistieken per vraag 
            foreach (var t in _tweets)
            {
                int uur = t.TweetDate.Hour;
                DateTime tweetijd = new DateTime();
                TimeSpan time = new TimeSpan(uur + 2, t.TweetDate.Minute, t.TweetDate.Second);
                tweetijd = new DateTime(t.TweetDate.Year, t.TweetDate.Month, t.TweetDate.Day, uur + 2, t.TweetDate.Minute, t.TweetDate.Second);  
              
                if (tweetijd > Program.deTijd)
                {
                    //variable region niet naar kijken --> vieze code voor t.vraagNr
                    #region Variable 
                    string title = t.Title;
                    title = title.Replace("@Inter_Smart", ""); //@Inter_Smart uit tweets verwijderen
                    title = title.Replace(" ", "");
                    title = title.Replace("\n", "");
                    string UpperCasetitle2 = title.ToUpper(); //Antwoorden in druk letters plaatsen voor score naar array zetten
                    UpperCasetitle2.ToCharArray();
                    string nr = "";
                    int intNr = 1;
                    char antw = 'z';

                    try
                    {
                        nr = UpperCasetitle2[0].ToString();
                        intNr = int.Parse(nr);
                        antw = UpperCasetitle2[1];
                    }
                    catch (Exception)
                    {
                        intNr = 999;
                        antw = 'z';
                      //  System.Console.WriteLine("typt da is juist !");
                    }
                    t.vraagNr = intNr;
                    #endregion 

                    int count = UpperCasetitle2.Count();
                    switch (count)
                    {
                        case 2:
                            if (t.vraagNr == deVraag)
                            {
                                if (UpperCasetitle2.Contains("A"))
                                {
                                    A += 1;
                                    totaal += 1;
                                }
                                else if (UpperCasetitle2.Contains("B"))
                                {
                                    B += 1;
                                    totaal += 1;
                                }
                                else if (UpperCasetitle2.Contains("C"))
                                {
                                    C += 1;
                                    totaal += 1;
                                }
                                else if (UpperCasetitle2.Contains("D"))
                                {
                                    D += 1;
                                    totaal += 1;
                                }
                            }
                            break;
                        case 3:
                            string nr2 = UpperCasetitle2[1].ToString();
                            antw = UpperCasetitle2[2];
                            string nummer = nr + nr2;
                            int nummerint = 1;
                            int tussewaarde = 0;
                            try
                            {
                                tussewaarde = Convert.ToInt32(nummer);
                            }
                            catch 
                            {
                                
                            }
                            t.vraagNr = tussewaarde;
                            try
                            {
                                nummerint = int.Parse(nummer);
                                //System.Console.WriteLine(nummerint);
                                //System.Console.ReadLine();
                            }
                            catch (Exception)

                            {
                                //System.Console.WriteLine(nummer);
                                //System.Console.ReadLine();
                            }
                            if (t.vraagNr == deVraag)
                            {
                                if (UpperCasetitle2.Contains("A"))
                                {
                                    A += 1;
                                    totaal += 1;
                                }
                                else if (UpperCasetitle2.Contains("B"))
                                {
                                    B += 1;
                                    totaal += 1;
                                }
                                else if (UpperCasetitle2.Contains("C"))
                                {
                                    C += 1;
                                    totaal += 1;
                                }
                                else if (UpperCasetitle2.Contains("D"))
                                {
                                    D += 1;
                                    totaal += 1;
                                }
                            }

                            break;
                    }
                }
                
            }
            resultatenLijst.Add(Environment.NewLine + "<set value=" + "\"" + A.ToString() + "\"" + "/>");
            resultatenLijst.Add(Environment.NewLine + "<set value=" + "\"" + B.ToString() + "\"" + "/>");
            resultatenLijst.Add(Environment.NewLine + "<set value=" + "\"" + C.ToString() + "\"" + "/>");
            resultatenLijst.Add(Environment.NewLine + "<set value=" + "\"" + D.ToString() + "\"" + "/>");
            #endregion 

            #region uitschrijven naar xml
            string textout = "";
            textout = "<?xml version=" + "\"1.0\"" + " encoding=" + "\"UTF-16\"" + " standalone=" + "\"yes\"" + "?>"
                + Environment.NewLine + "<chart caption=" + "\"Statistieken van vraag: "+ Program.intVerder.ToString() +"\"" + " subcaption=" + "\"" + filename + "\"" + " animation=" + "\"1\"" + " xaxisname=" + "\"Gestelde vragen\"" + " yaxisname=" + "\"Aantal\"" + ">"
                + Environment.NewLine + "<categories>";
            string textout2 = "";
            textout2 = "<category label=" + "\"" + "A" + "\"" + "/>" + Environment.NewLine + "<category label=" + "\"" + "B" + "\"" + "/>" + Environment.NewLine + "<category label=" + "\"" + "C" + "\"" + "/>" + Environment.NewLine + "<category label=" + "\"" + "D" + "\"" + "/>" + Environment.NewLine + "</categories>" + Environment.NewLine + "<dataset seriesName=" + "\"Antwoorden van deelnemers\"" + ">";
            string textout3 = "";
            textout3 = Environment.NewLine + "</dataset>"
                + Environment.NewLine + "<PP_Internal>"
                + Environment.NewLine + "<chart animation=" + "\"1\"" + " themename=" + "\"Custom\"" + "/>"
                + Environment.NewLine + "</PP_Internal>"
                + Environment.NewLine + "</chart>";
            file3.WriteLine(textout.ToString());
            file3.WriteLine(textout2.ToString());
            resultatenLijst.ForEach(file3.WriteLine);
            file3.WriteLine(textout3.ToString());
            resultatenLijst.Clear();
            file3.Close();
            #endregion 
        }
       
    
       
    }
}
