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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFGallery.Controls
{
    /// <summary>
    /// Interaction logic for TileGallery.xaml
    /// </summary>
    public partial class TileGallery : UserControl
    {
        public TileGallery()
        {
            InitializeComponent();
        }

        private void ScrollBackButton_Click(object sender, RoutedEventArgs e)
        {
            double newOffSet = RootScrollViewer.HorizontalOffset - 210;
            RootScrollViewer.ScrollToHorizontalOffset(newOffSet);
            UpdateScrollButtonsVisibility();
        }

        private void ScrollForwardButton_Click(object sender, RoutedEventArgs e)
        {
            double newOffSet = RootScrollViewer.HorizontalOffset + 210;
            RootScrollViewer.ScrollToHorizontalOffset(newOffSet);
            UpdateScrollButtonsVisibility();
        }

        private void UpdateScrollButtonsVisibility()
        {
            ScrollBackButton.Visibility = Visibility.Visible;
            ScrollForwardButton.Visibility = Visibility.Visible;

            if (RootScrollViewer.ActualWidth < TilesPanel.ActualWidth)
            {
                if(RootScrollViewer.HorizontalOffset == 0)
                {
                    ScrollBackButton.Visibility = Visibility.Collapsed;
                }
                else if(RootScrollViewer.HorizontalOffset >= RootScrollViewer.ScrollableWidth)
                {
                    ScrollForwardButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ScrollBackButton.Visibility = Visibility.Collapsed;
                ScrollForwardButton.Visibility = Visibility.Collapsed;
            }
        }

        private void RootScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScrollButtonsVisibility();
        }
    }
}
