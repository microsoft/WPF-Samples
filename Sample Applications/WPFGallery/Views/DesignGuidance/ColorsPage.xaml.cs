﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for ColorsPage.xaml
    /// </summary>
    public partial class ColorsPage : Page
    {
        public ColorsPageViewModel ViewModel { get; }
        public ColorsPage(ColorsPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PageSelector.SelectedIndex)
            {
                case 0:
                    ColorSubpageNavigationFrame.Navigate(new TextSection());
                    break;
                case 1:
                    ColorSubpageNavigationFrame.Navigate(new FillSection());
                    break;
                case 2:
                    ColorSubpageNavigationFrame.Navigate(new StrokeSection());
                    break;
                case 3:
                    ColorSubpageNavigationFrame.Navigate(new BackgroundSection());
                    break;
                case 4:
                    ColorSubpageNavigationFrame.Navigate(new SignalSection());
                    break;
                case 5:
                    ColorSubpageNavigationFrame.Navigate(new HighContrastSection());
                    break;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PageSelector.SelectedItem = PageSelector.Items[0];
        }
    }
}
