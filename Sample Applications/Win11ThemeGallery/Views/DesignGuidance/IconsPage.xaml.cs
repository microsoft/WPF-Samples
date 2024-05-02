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
using Win11ThemeGallery.ViewModels;
using Win11ThemeGallery.Models;

namespace Win11ThemeGallery.Views
{
    /// <summary>
    /// Interaction logic for IconsPage.xaml
    /// </summary>
    public partial class IconsPage : Page
    {
        public IconsPage(IconsPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        public IconsPageViewModel ViewModel { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                if(ViewModel.Icons.Count != IconsDataSource.GetIconsData().Count)
                {
                    ViewModel.Icons = IconsDataSource.GetIconsData();
                }
            }));
        }
    }
}
