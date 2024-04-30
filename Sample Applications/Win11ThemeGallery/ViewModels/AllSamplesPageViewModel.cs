using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using System.Windows.Controls;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class AllSamplesPageViewModel : BaseNavigablePageViewModel
    {
        public AllSamplesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "All Controls";
            PageDescription = "";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "Button",
                    PageType = typeof(ButtonPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Button.png"))},
                    Description = "A control that responds to user input and raises a Click event."
                },
                new NavigationCard
                {
                    Name = "CheckBox",
                    PageType = typeof(CheckBoxPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Checkbox.png"))},
                    Description = "A control that a user can select or clear."
                },
                new NavigationCard
                {
                    Name = "ComboBox",
                    PageType = typeof(ComboBoxPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ComboBox.png"))},
                    Description = "A drop-down list of items a user can select from."
                },
                new NavigationCard
                {
                    Name = "RadioButton",
                    PageType = typeof(RadioButtonPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/RadioButton.png"))},
                    Description = "A control that allows a user to select a single option from a group of options."
                },
                new NavigationCard
                {
                    Name = "Slider",
                    PageType = typeof(SliderPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Slider.png"))},
                    Description = "A control that lets the user select from a range of values by moving a Thumb control along a track."
                },
                new NavigationCard
                {
                    Name = "Data Grid",
                    PageType = typeof(DataGridPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DataGrid.png"))},
                    Description = "The DataGrid control presents data in a customizable table of rows and columns."
                },
                new NavigationCard
                {
                    Name = "ListBox",
                    PageType = typeof(ListBoxPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListBox.png"))},
                    Description = "A control that presents an inline list of items that the user can select from."
                },
                new NavigationCard
                {
                    Name = "ListView",
                    PageType = typeof(ListViewPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListView.png"))},
                    Description = "A control that presents a collection of items in a vertical list."
                },
                new NavigationCard
                {
                    Name = "TreeView",
                    PageType = typeof(TreeViewPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TreeView.png"))},
                    Description = "The TreeView control is a hierarchical list pattern with expanding and collapsing nodes that contain nested items."
                },
                new NavigationCard
                {
                    Name = "Calendar",
                    PageType = typeof(CalendarPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/CalendarView.png"))},
                    Description = "A control that presents a calendar for a user to choose a date from."
                },
                new NavigationCard
                {
                    Name = "DatePicker",
                    PageType = typeof(DatePickerPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DatePicker.png"))},
                    Description = "A control that lets a user pick a date value."
                },
                new NavigationCard
                {
                    Name = "Expander",
                    PageType = typeof(ExpanderPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Expander.png"))},
                    Description = "A container with a header that can be expanded to show a body with more content."
                },
                new NavigationCard
                {
                    Name = "Menu",
                    PageType = typeof(MenuPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Pivot.png"))},
                    Description = "A classic menu, allowing the display of MenuItems containing MenuFlyoutItems."
                },
                new NavigationCard
                {
                    Name = "TabControl",
                    PageType = typeof(TabControlPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TabView.png"))},
                    Description = "A control that displays a collection of tabs."
                },
                new NavigationCard
                {
                    Name = "ProgressBar",
                    PageType = typeof(ProgressBarPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ProgressBar.png"))},
                    Description = "Shows the apps progress on a task, or that the app is performing ongoing work that doesn't block user interaction."
                },
                new NavigationCard
                {
                    Name = "ToolTip",
                    PageType = typeof(ToolTipPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ToolTip.png"))},
                    Description = "Displays information for an element in a pop-up window."
                },
                new NavigationCard
                {
                    Name = "Label",
                    PageType = typeof(LabelPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Button.png"))},
                    Description = "Caption of an item."
                },
                new NavigationCard
                {
                    Name = "TextBlock",
                    PageType = typeof(TextBlockPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TextBlock.png"))},
                    Description = "A lightweight control for displaying small amounts of text."
                },
                new NavigationCard
                {
                    Name = "TextBox",
                    PageType = typeof(TextBoxPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TextBox.png"))},
                    Description = "A single-line or multi-line plain text field."
                },
                new NavigationCard
                {
                    Name = "RichTextEdit",
                    PageType = typeof(RichTextEditPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/RichEditBox.png"))},
                    Description = "A control that displays formatted text, hyperlinks, inline images, and other rich content."
                },
                new NavigationCard
                {
                    Name = "PasswordBox",
                    PageType = typeof(PasswordBoxPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/PasswordBox.png"))},
                    Description = "A control for entering passwords."
                },
            };
        }
    }
}
