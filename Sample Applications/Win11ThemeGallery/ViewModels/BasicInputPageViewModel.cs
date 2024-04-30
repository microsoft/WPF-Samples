using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class BasicInputPageViewModel : BaseNavigablePageViewModel 
    {
        public BasicInputPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Basic Input";
            PageDescription = "Controls for getting user input";
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
            };
        }
    }
}
