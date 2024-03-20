using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11ThemeGallery.ViewModels
{
    public partial class RichTextEditPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "RichTextEdit";

	[ObservableProperty]
	private string _pageDescription = "";

    }
}
