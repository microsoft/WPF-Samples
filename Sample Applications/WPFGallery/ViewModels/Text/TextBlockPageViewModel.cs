using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels
{
    public partial class TextBlockPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "TextBlock";

	[ObservableProperty]
	private string _pageDescription = "";

    }
}
