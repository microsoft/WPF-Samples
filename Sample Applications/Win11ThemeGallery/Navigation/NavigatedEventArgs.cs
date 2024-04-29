namespace Win11ThemeGallery.Navigation
{
    public class NavigatingEventArgs
    {
        public Type? PageType { get; set; } = null;

        public NavigatingEventArgs() { }

        public NavigatingEventArgs(Type pageType)
        {
            PageType = pageType;
        }
    }
}