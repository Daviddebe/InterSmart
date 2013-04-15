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
using System.Windows.Threading;

namespace InterSmart.Pages
{
    /// <summary>
    /// Interaction logic for QuestionPopUp.xaml
    /// </summary>
    public partial class QuestionPopUp : Window
    {
        public QuestionPopUp()
        {
            InitializeComponent();
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
                this.Left = corner.X - this.ActualWidth - 50;
                this.Top = corner.Y - this.ActualHeight;

            }));
        }
    }
}
