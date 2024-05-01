using System;
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
    public partial class BasicInputPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Basic Input";

        [ObservableProperty]
        private string _pageDescription = "Controls for getting user input";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Button",
                PageType = typeof(ButtonPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Button.png"))},
                //Icon = new SymbolIcon { Symbol = SymbolRegular.ControlButton24 },
                Description = "A control that responds to user input and raises a Click event."
            },
            new NavigationCard
            {
                Name = "CheckBox",
                PageType = typeof(CheckBoxPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Checkbox.png"))},
                //Icon = new SymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                Description = "A control that a user can select or clear."
            },
            new NavigationCard
            {
                Name = "ComboBox",
                PageType = typeof(ComboBoxPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ComboBox.png"))},
                //Icon = new SymbolIcon { Symbol = SymbolRegular.Filter16 },
                Description = "A drop-down list of items a user can select from."
            },
            new NavigationCard
            {
                Name = "RadioButton",
                PageType = typeof(RadioButtonPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/RadioButton.png"))},
                //Icon = new SymbolIcon { Symbol = SymbolRegular.RadioButton24 },
                Description = "A control that allows a user to select a single option from a group of options."
            },
            new NavigationCard
            {
                Name = "Slider",
                PageType = typeof(SliderPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Slider.png"))},
                //Icon = new SymbolIcon { Symbol = SymbolRegular.HandDraw24 },
                Description = "A control that lets the user select from a range of values by moving a Thumb control along a track."
            },

        };

        private readonly INavigationService _navigationService;

        public BasicInputPageViewModel(INavigationService navigationService)
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
