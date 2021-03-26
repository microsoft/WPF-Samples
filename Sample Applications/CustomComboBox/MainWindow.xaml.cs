using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomComboBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (e.Property == TextBlock.TextProperty)
            {
                var textBlockPeer = UIElementAutomationPeer.FromElement(sender as TextBlock);
                if(textBlockPeer != null)
                {
                    textBlockPeer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                }
            }
        }
    }
}
