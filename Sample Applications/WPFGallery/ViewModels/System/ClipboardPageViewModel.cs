
namespace WPFGallery.ViewModels;

public partial class ClipboardPageViewModel : ObservableObject 
{
    [ObservableProperty]
    private string _pageTitle = "Clipboard";

    [ObservableProperty]
    private string _pageDescription = "";

    [ObservableProperty]
    private string _copyStatus = "";

    [ObservableProperty]
    private string _pastedText = "";

    [ObservableProperty]
    private string _clearStatus = "";

    [ObservableProperty]
    private string _formatsInfo = "";

    [ObservableProperty]
    private string _copyImageStatus = "";

    [ObservableProperty]
    private string _pasteImageStatus = "";

    public ClipboardPageViewModel() { }
}
