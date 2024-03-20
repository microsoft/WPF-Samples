using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Win11ThemeGallery.ViewModels;

public partial class ImagePageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Image";

	[ObservableProperty]
	private string _pageDescription = "";


}
