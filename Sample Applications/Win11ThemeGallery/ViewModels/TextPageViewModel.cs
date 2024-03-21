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
    public partial class TextPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Text Controls";

        [ObservableProperty]
        private string _pageDescription = "Controls for displaying and editing text";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Label",
                PageType = typeof(LabelPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TextBaseline20 },
                Description = "Caption of an item"
            },
            new NavigationCard
            {
                Name = "TextBlock",
                PageType = typeof(TextBlockPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TextCaseLowercase24 },
                Description = "Control for displaying text"
            },
            new NavigationCard
            {
                Name = "TextBox",
                PageType = typeof(TextBoxPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TextColor24 },
                Description = "Plain text field"
            },
            new NavigationCard
            {
                Name = "RichTextEdit",
                PageType = typeof(RichTextEditPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.DrawText24 },
                Description = "A rich text editing control"
            },
            new NavigationCard
            {
                Name = "PasswordBox",
                PageType = typeof(PasswordBoxPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.Password24 },
                Description = "A control for entering passwords"
            },
        };

        private readonly INavigationService _navigationService;

        public TextPageViewModel(INavigationService navigationService)
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
