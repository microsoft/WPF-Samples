using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11ThemeGallery.ViewModels
{
    public partial class TextBoxPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "TextBox";

	[ObservableProperty]
	private string _pageDescription = "";

    }
}
