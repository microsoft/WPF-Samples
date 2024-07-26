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

namespace WPFGallery.Views;
    /// <summary>
    /// Interaction logic for RadioButtonPage.xaml
    /// </summary>
    public partial class RadioButtonPage : Page
    {
    public RadioButtonPageViewModel ViewModel { get; }

    public RadioButtonPage(RadioButtonPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    private void RadioButton_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        if (radioButton != null)
        {
            radioButton.IsChecked = true;
        }
    }
}