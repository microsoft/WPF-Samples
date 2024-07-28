using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFGallery.ViewModels;

public partial class ListBoxPageViewModel : BasePageViewModel 
{
    [ObservableProperty]
    private ObservableCollection<string> _listBoxItems;

    public ListBoxPageViewModel() : base("ListBox")
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