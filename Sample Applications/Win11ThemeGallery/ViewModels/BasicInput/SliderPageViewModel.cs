using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11ThemeGallery.ViewModels;


public partial class SliderPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Slider";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private int _simpleSliderValue = 0;

    [ObservableProperty]
    private int _rangeSliderValue = 500;

    [ObservableProperty]
    private int _marksSliderValue = 0;

    [ObservableProperty]
    private int _verticalSliderValue = 0;
}

