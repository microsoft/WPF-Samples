using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
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
                //Icon = new SymbolIcon { Symbol = SymbolRegular.ControlButton24 },
                Description = "Simple Button"
            },
            new NavigationCard
            {
                Name = "CheckBox",
                PageType = typeof(CheckBoxPage),
                //Icon = new SymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                Description = "Button with binary choice"
            },
            new NavigationCard
            {
                Name = "ComboBox",
                PageType = typeof(ComboBoxPage),
                //Icon = new SymbolIcon { Symbol = SymbolRegular.Filter16 },
                Description = "Select item from a list"
            },
            new NavigationCard
            {
                Name = "RadioButton",
                PageType = typeof(RadioButtonPage),
                //Icon = new SymbolIcon { Symbol = SymbolRegular.RadioButton24 },
                Description = "Set of mutually exclusive choices"
            },
            new NavigationCard
            {
                Name = "Slider",
                PageType = typeof(SliderPage),
                //Icon = new SymbolIcon { Symbol = SymbolRegular.HandDraw24 },
                Description = "Sliding value selector"
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
