using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Win11ThemeGallery.ViewModels;

public partial class ListBoxPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "ListBox";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private ObservableCollection<string> _listBoxItems;

    public ListBoxPageViewModel()
    {
        _listBoxItems = new ObservableCollection<string>
        {
            "Arial",
            "Comic Sans MS",
            "Courier New",
            "Segoe UI",
            "Times New Roman"
        };
    }
}