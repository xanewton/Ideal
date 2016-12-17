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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Ideal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static RoutedUICommand ExitCommand =
            new RoutedUICommand("Exit", "Exit", typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExecuteExitCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MyFrameNavigated(object sender, NavigationEventArgs e)
        {
            var myFadeInAnimation = (DoubleAnimation)Resources["MyFadeInAnimationResource"];
            myFrame.BeginAnimation(OpacityProperty, myFadeInAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        private void TransitionAnimationStateChanged(object sender, EventArgs e)
        {
            var transitionAnimationClock = (AnimationClock)sender;
            if (transitionAnimationClock.CurrentState == ClockState.Filling)
            {
                FadeEnded();
            }
        }

        private void FadeEnded()
        {
            var el = (XmlElement)myPageList.SelectedItem;
            var att = el.Attributes["Uri"];
            if (att != null)
            {
                myFrame.Navigate(new Uri(att.Value, UriKind.Relative));
            }
            else
            {
                myFrame.Content = null;
            }
        }

    }
}

