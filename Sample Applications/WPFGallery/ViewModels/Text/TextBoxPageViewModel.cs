using System.ComponentModel.DataAnnotations;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WPFGallery.ViewModels
{
    public partial class TextBoxPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "TextBox";

        [ObservableProperty]
        private string _pageDescription = "";

        [ObservableProperty]
        private string _validatedText = string.Empty;
    }
}
