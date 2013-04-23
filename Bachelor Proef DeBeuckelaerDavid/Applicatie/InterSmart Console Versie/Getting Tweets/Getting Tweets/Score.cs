using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getting_Tweets
{
    public class Score
    {
        static getTweets tweet = new getTweets();
        static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        private List<int> AnswerList = new List<int>();

        public void score()
        {
            _tweets = tweet.GetCollection();
            int scoren = 0;
            string Antwoord1 = Program.Antwoorden[0].ToString();
            string Antwoord2 = Program.Antwoorden[1].ToString();
            string Antwoord3 = Program.Antwoorden[2].ToString();
            string Antwoord4 = Program.Antwoorden[3].ToString();
            string Antwoord5 = Program.Antwoorden[4].ToString();


            foreach (var t in _tweets)
            {
                string cut = t.Title;
                cut = cut.Substring(0, cut.Length - 12);//@Intersmart van tweets afknippen
                string cutUpperCase = cut.ToUpper(); //naar druk letters zetten

                if (cutUpperCase.Contains("1")) //vraag 1
                {
                    if (cutUpperCase.Contains(Antwoord1.ToUpper()))
                    {
                        scoren++;
                        Console.WriteLine(cutUpperCase.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Fout");
                    }
                }
                else if (cutUpperCase.Contains("2")) //vraag 2
                {
                    if (cutUpperCase.Contains(Antwoord2.ToUpper()))
                    {
                        scoren++;
                        Console.WriteLine(cutUpperCase.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Fout");
                    }
                }
                else if (cutUpperCase.Contains("3")) //vraag 3
                {
                    if (cutUpperCase.Contains(Antwoord3.ToUpper()))
                    {
                        scoren++;
                        Console.WriteLine(cutUpperCase.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Fout");
                    }
                }
                else if (cutUpperCase.Contains("4")) //vraag 4
                {
                    if (cutUpperCase.Contains(Antwoord4.ToUpper()))
                    {
                        scoren++;
                        Console.WriteLine(cutUpperCase.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Fout");
                    }
                }
                else if (cutUpperCase.Contains("5")) //vraag 5
                {
                    if (cutUpperCase.Contains(Antwoord5.ToUpper()))
                    {
                        scoren++;
                        Console.WriteLine(cutUpperCase.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Fout");
                    }
                }
                Console.WriteLine(scoren.ToString());
            }

            Console.ReadLine();
        }

    }
}
