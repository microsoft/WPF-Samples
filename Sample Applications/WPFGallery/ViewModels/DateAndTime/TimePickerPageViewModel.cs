using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFGallery.ViewModels;

public partial class TimePickerPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "TimePicker";

	[ObservableProperty]
	private string _pageDescription = "";


}
