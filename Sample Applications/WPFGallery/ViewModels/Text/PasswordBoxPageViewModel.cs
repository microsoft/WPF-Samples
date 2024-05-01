using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels
{
    public partial class PasswordBoxPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "PasswordBox";

	[ObservableProperty]
	private string _pageDescription = "";

    }
}
