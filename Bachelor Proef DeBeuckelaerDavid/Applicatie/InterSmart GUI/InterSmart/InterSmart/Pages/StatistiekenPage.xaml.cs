﻿using InterSmart.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace InterSmart.Pages
{
    /// <summary>
    /// Interaction logic for StatistiekenPage.xaml
    /// </summary>
    public partial class StatistiekenPage : Window
    {
        #region Variable
        public static getTweets tweet;
        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        public List<int> AnswerList = new List<int>();
        #endregion

        public StatistiekenPage()
        {
            InitializeComponent();
        }

        #region Button Events
        private void BtnShowStats_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //simpele berekening voor % van een antwoord.
                AnswerList = MainPage.tweet.GetAnswers();
                double totaal = AnswerList[4];
                double berekeningA = (AnswerList[0] / totaal * 100);
                double berekeningB = (AnswerList[1] / totaal * 100);
                double berekeningC = (AnswerList[2] / totaal * 100);
                double berekeningD = (AnswerList[3] / totaal * 100);
                lstbxStats.Items.Clear();
                lstbxStats.Items.Add("A = " + (AnswerList[0]) + " --> " + Math.Round(berekeningA, 2) + "%"); //math round is voor 2 cijfers na de komma
                lstbxStats.Items.Add("B = " + (AnswerList[1]) + " --> " + Math.Round(berekeningB, 2) + "%");
                lstbxStats.Items.Add("C = " + (AnswerList[2]) + " --> " + Math.Round(berekeningC, 2) + "%");
                lstbxStats.Items.Add("D = " + (AnswerList[3]) + " --> " + Math.Round(berekeningD, 2) + "%");
                lstbxStats.Items.Add("Totaal = " + AnswerList[4]);
                AnswerList.Clear();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnGraph_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string inputVraag = Microsoft.VisualBasic.Interaction.InputBox("Geef vraag nummer in", "Input Question number", "...", 50, 60);
                AnswerList = MainPage.tweet.GetAnswers();
                double A = AnswerList[0];
                double B = AnswerList[1];
                double C = AnswerList[2];
                double D = AnswerList[3];
                OpenFileDialog f = new OpenFileDialog();
                f.ShowDialog();
                //opmaak txt voor grafiek 
                string textout = "";
                textout = "<?xml version=" + "\"1.0\"" + " encoding=" + "\"UTF-16\"" + " standalone=" + "\"yes\"" + "?>"
                    + Environment.NewLine + "<chart caption=" + "\"Aantal Antwoorden\"" + " subcaption=" + "\"" + inputVraag + "\"" + " animation=" + "\"1\"" + " xaxisname=" + "\"Antwoorden\"" + " yaxisname=" + "\"Aantal\"" + ">"
                    + Environment.NewLine + "<categories>"
                    + Environment.NewLine + "<category label=" + "\"Antwoord A\"" + "/>"
                    + Environment.NewLine + "<category label=" + "\"Antwoord B\"" + "/>"
                    + Environment.NewLine + "<category label=" + "\"Antwoord C\"" + "/>"
                    + Environment.NewLine + "<category label=" + "\"Antwoord D\"" + "/>"
                    + Environment.NewLine + "</categories>"
                    + Environment.NewLine + "<dataset seriesName=" + "\"Aantal\"" + ">"
                    + Environment.NewLine + "<set value=" + "\"" + A + "\"" + "/>"
                    + Environment.NewLine + "<set value=" + "\"" + B + "\"" + "/>"
                    + Environment.NewLine + "<set value=" + "\"" + C + "\"" + "/>"
                    + Environment.NewLine + "<set value=" + "\"" + D + "\"" + "/>"
                    + Environment.NewLine + "</dataset>"
                    + Environment.NewLine + "<PP_Internal>"
                    + Environment.NewLine + "<chart animation=" + "\"1\"" + " themename=" + "\"Custom\"" + "/>"
                    + Environment.NewLine + "</PP_Internal>"
                    + Environment.NewLine + "</chart>";
                File.WriteAllText(f.FileName, textout);
                MessageBox.Show("Load Succesvol");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        #endregion

    }
}
