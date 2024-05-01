﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class DashboardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Basic Input",
                PageType = typeof(BasicInputPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Button.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                Description = "Button, CheckBox, ComboBox, RadioButton, Slider."
            },
            new NavigationCard
            {
                Name = "Collections",
                PageType = typeof(CollectionsPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DataGrid.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.Table24 },
                Description = "DataGrid, ListBox, ListView, TreeView."
            },
            new NavigationCard
            {
                Name = "Date & Time",
                PageType = typeof(DateAndTimePage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/CalendarView.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CalendarClock24 },
                Description = "Calendar, DatePicker"
            },
            new NavigationCard
            {
                Name = "Layout",
                PageType = typeof(LayoutPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Expander.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.News24 },
                Description = "Expander"
            },
            // new NavigationCard
            // {
            //     Name = "Media",
            //     PageType = typeof(MediaPage),
            //     Icon = newSymbolIcon { Symbol = SymbolRegular.PlayCircle24 },
            //     Description = "Canvas, Image"
            // },
            new NavigationCard
            {
                Name = "Navigation",
                PageType = typeof(NavigationPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/MenuBar.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.Navigation24 },
                Description = "Menu, TabControl"
            },
            new NavigationCard
            {
                Name = "Status & Info",
                PageType = typeof(StatusAndInfoPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ProgressBar.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.ChatBubblesQuestion24 },
                Description = "ProgressBar, ToolTip"
            },
            new NavigationCard
            {
                Name = "Text",
                PageType = typeof(TextPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TextBlock.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.DrawText24 },
                Description = "Label, TextBlock, TextBox, RichTextEdit, PasswordBox."
            },
        };

        private readonly INavigationService _navigationService;

        public DashboardPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Navigate(object pageType){
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }

        
    }
}
