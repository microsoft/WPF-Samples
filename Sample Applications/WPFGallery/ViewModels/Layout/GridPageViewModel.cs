namespace WPFGallery.ViewModels.Layout
{
    public partial class GridPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Grid";

        [ObservableProperty]
        private string _pageDescription = "";

        public GridPageViewModel() { }
    }
}
