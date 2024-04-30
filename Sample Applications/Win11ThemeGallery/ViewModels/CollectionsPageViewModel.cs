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
    public partial class CollectionsPageViewModel : BaseNavigablePageViewModel
    {
        public CollectionsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Collection Controls";
            PageDescription = "Controls for collection presentation";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "Data Grid",
                    PageType = typeof(DataGridPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DataGrid.png"))},
                    Description = "The DataGrid control presents data in a customizable table of rows and columns."
                },
                new NavigationCard
                {
                    Name = "ListBox",
                    PageType = typeof(ListBoxPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListBox.png"))},
                    Description = "A control that presents an inline list of items that the user can select from."
                },
                new NavigationCard
                {
                    Name = "ListView",
                    PageType = typeof(ListViewPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListView.png"))},
                    Description = "A control that presents a collection of items in a vertical list."
                },
                new NavigationCard
                {
                    Name = "TreeView",
                    PageType = typeof(TreeViewPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TreeView.png"))},
                    Description = "The TreeView control is a hierarchical list pattern with expanding and collapsing nodes that contain nested items."
                },
            };
        }
    }
}
