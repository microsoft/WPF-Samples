using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for SliderPage.xaml
    /// </summary>
    public partial class SliderPage : Page
    {
        public SliderPageViewModel ViewModel { get; }

        private ToolTip _simpleToolTip;
        private ToolTip _rangeToolTip;
        private ToolTip _ticksToolTip;
        private ToolTip _verticalToolTip;
        private Dictionary<Slider, ToolTip> TooltipDict = new Dictionary<Slider, ToolTip>();

        public SliderPage(SliderPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();           

            _simpleToolTip = new ToolTip();
            simple_slider.ToolTip = _simpleToolTip;
            _simpleToolTip.Content = simple_slider.Minimum;
            TooltipDict.Add(simple_slider,_simpleToolTip);

            _rangeToolTip = new ToolTip();
            range_slider.ToolTip = _rangeToolTip;
            _rangeToolTip.Content = range_slider.Minimum;
            TooltipDict.Add(range_slider, _rangeToolTip);
            
            _ticksToolTip = new ToolTip();
            ticks_slider.ToolTip = _ticksToolTip;
            _ticksToolTip.Content = simple_slider.Minimum;
            TooltipDict.Add(ticks_slider, _ticksToolTip);

            _verticalToolTip = new ToolTip();
            vertical_slider.ToolTip = _verticalToolTip;
            _verticalToolTip.Content = vertical_slider.Minimum;
            TooltipDict.Add(vertical_slider, _verticalToolTip);

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
            if (sender is Slider slider) 
            {
                if (!TooltipDict.ContainsKey(slider))
                {
                    return;
                }
              
                else
                {
                    ToolTip tooltip = TooltipDict[slider];
                    tooltip.Content = e.NewValue;
                    if (!tooltip.IsOpen)
                    {
                        tooltip.IsOpen = true;
                    }

                }
            }
            

        }

        private void Slider_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Slider slider)
            {
                ToolTip tooltip = TooltipDict[slider];
                if (tooltip != null)
                {
                    tooltip.IsOpen = true;
                }
            }    
            
        }

        private void Slider_MouseLeave(object sender, MouseEventArgs e)
        {

            if (sender is Slider slider)
            {
                ToolTip tooltip = TooltipDict[slider];
                if (tooltip != null)
                {
                    tooltip.IsOpen = false;
                }
            }
            

        }
    }
}
