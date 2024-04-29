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
