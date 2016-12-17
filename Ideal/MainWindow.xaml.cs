﻿#region Copyright
/*
 * Copyright (C) 2016 Angel Garcia
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion
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

