using System.Windows;
using System.Windows.Media;
using System.Windows.Forms.Integration;
using WPFControls;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Windows.FontStyle;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private ElementHost? _elementHost;
        private SolidColorBrush? _initialBackground;
        private SolidColorBrush? _initialForeground;
        private FontFamily? _initialFontFamily;
        private double _initialFontSize = 8;
        private FontStyle _initialFontStyle;
        private FontWeight _initialFontWeight;
        private MyControl? _wpfControl;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeWpfControl();
        }

        private void InitializeWpfControl()
        {
            _elementHost = new ElementHost { Dock = DockStyle.Fill };
            panel1.Controls.Clear();
            panel1.Controls.Add(_elementHost);

            _wpfControl = new MyControl();
            _wpfControl.OnButtonClick += WpfControl_OnButtonClick;
            _wpfControl.Loaded += WpfControl_Loaded;
            _elementHost.Child = _wpfControl;
        }

        private void WpfControl_Loaded(object sender, EventArgs e)
        {
            if (_wpfControl is null)
            {
                return;
            }

            _initialBackground = _wpfControl.MyControlBackground;
            _initialForeground = _wpfControl.MyControlForeground;
            _initialFontFamily = _wpfControl.MyControlFontFamily;
            _initialFontSize = _wpfControl.MyControlFontSize;
            _initialFontStyle = _wpfControl.MyControlFontStyle;
            _initialFontWeight = _wpfControl.MyControlFontWeight;

            radioBackgroundOriginal.Checked = true;
            radioForegroundOriginal.Checked = true;
            radioFamilyOriginal.Checked = true;
            radioSizeOriginal.Checked = true;
            radioStyleOriginal.Checked = true;
            radioWeightOriginal.Checked = true;
        }

        private void WpfControl_OnButtonClick(object? sender, MyControlEventArgs args)
        {
            if (args.IsOk)
            {
                lblName.Text = $"Name: {args.MyName}";
                lblAddress.Text = $"Street Address: {args.MyStreetAddress}";
                lblCity.Text = $"City: {args.MyCity}";
                lblState.Text = $"State: {args.MyState}";
                lblZip.Text = $"Zip: {args.MyZip}";
            }
            else
            {
                ResetLabels();
            }
        }

        private void ResetLabels()
        {
            lblName.Text = "Name: ";
            lblAddress.Text = "Street Address: ";
            lblCity.Text = "City: ";
            lblState.Text = "State: ";
            lblZip.Text = "Zip: ";
        }

        private bool IsControlReady() => _wpfControl is not null;

        private void radioBackgroundOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioBackgroundOriginal.Checked || !IsControlReady() || _initialBackground is null)
            {
                return;
            }

            _wpfControl!.MyControlBackground = _initialBackground;
        }

        private void radioBackgroundLightGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBackgroundLightGreen.Checked && IsControlReady())
            {
                _wpfControl!.MyControlBackground = new SolidColorBrush(Colors.LightGreen);
            }
        }

        private void radioBackgroundLightSalmon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBackgroundLightSalmon.Checked && IsControlReady())
            {
                _wpfControl!.MyControlBackground = new SolidColorBrush(Colors.LightSalmon);
            }
        }

        private void radioForegroundOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioForegroundOriginal.Checked || !IsControlReady() || _initialForeground is null)
            {
                return;
            }

            _wpfControl!.MyControlForeground = _initialForeground;
        }

        private void radioForegroundRed_CheckedChanged(object sender, EventArgs e)
        {
            if (radioForegroundRed.Checked && IsControlReady())
            {
                _wpfControl!.MyControlForeground = new SolidColorBrush(Colors.Red);
            }
        }

        private void radioForegroundYellow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioForegroundYellow.Checked && IsControlReady())
            {
                _wpfControl!.MyControlForeground = new SolidColorBrush(Colors.Yellow);
            }
        }

        private void radioFamilyOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioFamilyOriginal.Checked || !IsControlReady() || _initialFontFamily is null)
            {
                return;
            }

            _wpfControl!.MyControlFontFamily = _initialFontFamily;
        }

        private void radioFamilyTimes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioFamilyTimes.Checked && IsControlReady())
            {
                _wpfControl!.MyControlFontFamily = new FontFamily("Times New Roman");
            }
        }

        private void radioFamilyWingDings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioFamilyWingDings.Checked && IsControlReady())
            {
                _wpfControl!.MyControlFontFamily = new FontFamily("WingDings");
            }
        }

        private void radioSizeOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioSizeOriginal.Checked || !IsControlReady())
            {
                return;
            }

            _wpfControl!.MyControlFontSize = _initialFontSize;
        }

        private void radioSizeTen_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSizeTen.Checked && IsControlReady())
            {
                _wpfControl!.MyControlFontSize = 10;
            }
        }

        private void radioSizeTwelve_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSizeTwelve.Checked && IsControlReady())
            {
                _wpfControl!.MyControlFontSize = 12;
            }
        }

        private void radioStyleOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioStyleOriginal.Checked || !IsControlReady())
            {
                return;
            }

            _wpfControl!.MyControlFontStyle = _initialFontStyle;
        }

        private void radioStyleItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioStyleItalic.Checked && IsControlReady())
            {
                _wpfControl!.MyControlFontStyle = FontStyles.Italic;
            }
        }

        private void radioWeightOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioWeightOriginal.Checked || !IsControlReady())
            {
                return;
            }

            _wpfControl!.MyControlFontWeight = _initialFontWeight;
        }

        private void radioWeightBold_CheckedChanged(object sender, EventArgs e)
        {
            if (radioWeightBold.Checked && IsControlReady())
            {
                _wpfControl!.MyControlFontWeight = FontWeights.Bold;
            }
        }
    }
}
