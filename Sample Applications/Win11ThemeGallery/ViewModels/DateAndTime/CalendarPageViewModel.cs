using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Win11ThemeGallery.ViewModels;

public partial class CalendarPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Calendar";

	[ObservableProperty]
	private string _pageDescription = "";


}
