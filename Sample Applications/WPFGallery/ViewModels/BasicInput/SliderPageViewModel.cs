namespace WPFGallery.ViewModels;

public partial class SliderPageViewModel : BasePageViewModel 
{
    public SliderPageViewModel() : base("Slider") {}
    
    [ObservableProperty]
    private int _simpleSliderValue = 0;

    [ObservableProperty]
    private int _rangeSliderValue = 500;

    [ObservableProperty]
    private int _marksSliderValue = 0;

    [ObservableProperty]
    private int _verticalSliderValue = 0;
}

