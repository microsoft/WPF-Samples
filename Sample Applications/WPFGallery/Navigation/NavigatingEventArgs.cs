using WPFGallery.Models;

namespace WPFGallery.Navigation
{
    /// <summary>
    /// Event arguments for the Navigating event.
    /// </summary>
    public class NavigatingEventArgs
    {
        public ControlInfoDataItem? DataItem { get; set; } = null;

        public NavigatingEventArgs() { }

        public NavigatingEventArgs(ControlInfoDataItem dataItem)
        {
            DataItem = dataItem;
        }
    }
}