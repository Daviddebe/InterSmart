using System;
using System.Collections.Generic;
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
using System.Xml;

namespace InterSmartV1._1.Menu
{
    /// <summary>
    /// Interaction logic for DataBase.xaml
    /// </summary>
    public partial class DataBase : Window
    {
        public DataBase()
        {
            InitializeComponent();
        }

        private void BtnProcess_Click_1(object sender, RoutedEventArgs e)
        {
            string path = @"TweetDB\" + txtbxZoek.Text.ToString() + ".xml";
            XmlReader reader = XmlReader.Create(path);
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "Person"))
                {
                    if (reader.HasAttributes)
                    {
                        RtbResult.Items.Add("Firstname:" + reader.GetAttribute("firstname") + '\n' + "lastname:" + reader.GetAttribute("lastname"));
                    }
                }
            }
            #region via txtbox
            //string path = @"TweetDB\" + txtbxZoek.Text.ToString() + ".xml";
            //XmlDocument document = new XmlDocument();
            //document.Load(path);
            //XmlNode node = document.SelectSingleNode(@"//*");
            //RtbResult.Text = node.OuterXml.ToString();
            #endregion 
            #region code via richtextbox
            //using (XmlReader reader = XmlReader.Create(path))
            //{
            //    FlowDocument myFlowDoc = new FlowDocument();

            //    while (reader.Read())
            //    {

            //        switch (reader.NodeType)
            //        {
            //            case XmlNodeType.Element: // The node is an element.
            //                myFlowDoc.Blocks.Add(new Paragraph(new Run("<" + reader.Name))); 
            //                while (reader.MoveToNextAttribute()) // Read the attributes.
            //                  myFlowDoc.Blocks.Add(new Paragraph(new Run(" " + reader.Name + "='" + reader.Value + "'")));
            //               myFlowDoc.Blocks.Add(new Paragraph(new Run (">")));
            //                break;
            //            case XmlNodeType.Text: //Display the text in each element.
            //               myFlowDoc.Blocks.Add(new Paragraph(new Run (reader.Value)));
            //                break;
            //            case XmlNodeType.EndElement: //Display the end of the element.
            //               myFlowDoc.Blocks.Add(new Paragraph(new Run("</" + reader.Name)));
            //               myFlowDoc.Blocks.Add(new Paragraph(new Run(">")));
            //                break;
            //        }
            //        RtbResult.Document = myFlowDoc;
                    
            //    }
            //}
            #endregion
        }

        private void BtnClear_Click_1(object sender, RoutedEventArgs e)
        {
           
            txtbxZoek.Text = "";
        }

       

    }
}
