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
    /// Interaction logic for LabelPage.xaml
    /// </summary>
    public partial class LabelPage : Page
    {
        public LabelPageViewModel ViewModel { get; }
public LabelPage(LabelPageViewModel viewModel)
        {
ViewModel = viewModel;
DataContext = this;
            InitializeComponent();
        }
    }
}
