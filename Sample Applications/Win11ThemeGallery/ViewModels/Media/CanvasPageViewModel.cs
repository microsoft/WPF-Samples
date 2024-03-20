using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Win11ThemeGallery.ViewModels;

public partial class CanvasPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Canvas";

	[ObservableProperty]
	private string _pageDescription = "";


}
