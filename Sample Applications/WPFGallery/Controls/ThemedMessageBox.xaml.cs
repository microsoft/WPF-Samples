using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFGallery.Controls
{
    /// <summary>
    /// A Fluent-themed replacement for <see cref="System.Windows.MessageBox"/>. Because the native
    /// Win32 message box does not honor the application's Fluent <c>ThemeMode</c>, it stays light in
    /// dark mode and fails contrast requirements. This dialog is a WPF <see cref="Window"/>, so it
    /// inherits the app theme (Light/Dark/High Contrast) and adapts automatically.
    /// </summary>
    public partial class ThemedMessageBox : Window
    {
        private MessageBoxResult _result = MessageBoxResult.None;
        private MessageBoxResult _cancelResult = MessageBoxResult.None;

        private ThemedMessageBox()
        {
            InitializeComponent();
        }

        public static MessageBoxResult Show(string messageBoxText)
            => Show(messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);

        public static MessageBoxResult Show(string messageBoxText, string caption)
            => Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
            => Show(messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None);

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
            => Show(messageBoxText, caption, button, icon, MessageBoxResult.None);

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            var dialog = new ThemedMessageBox();
            dialog.Build(messageBoxText, caption, button, icon, defaultResult);

            Window? owner = Application.Current?.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive) ?? Application.Current?.MainWindow;

            if (owner != null && owner != dialog && owner.IsLoaded)
            {
                dialog.Owner = owner;
            }
            else
            {
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            dialog.ShowDialog();
            return dialog._result;
        }

        private void Build(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            Title = caption ?? string.Empty;
            MessageText.Text = messageBoxText ?? string.Empty;
            AutomationProperties.SetName(this, string.IsNullOrEmpty(caption) ? "Message" : caption);

            ApplyIcon(icon);
            BuildButtons(button, defaultResult);
        }

        private void ApplyIcon(MessageBoxImage icon)
        {
            string glyph;
            string brushKey;
            string name;

            switch (icon)
            {
                case MessageBoxImage.Error: // also Stop, Hand
                    glyph = "\uEA39";
                    brushKey = "SystemFillColorCriticalBrush";
                    name = "Error";
                    break;
                case MessageBoxImage.Warning: // also Exclamation
                    glyph = "\uE7BA";
                    brushKey = "SystemFillColorCautionBrush";
                    name = "Warning";
                    break;
                case MessageBoxImage.Information: // also Asterisk
                    glyph = "\uE946";
                    brushKey = "SystemFillColorAttentionBrush";
                    name = "Information";
                    break;
                case MessageBoxImage.Question:
                    glyph = "\uE9CE";
                    brushKey = "SystemFillColorAttentionBrush";
                    name = "Question";
                    break;
                default:
                    IconGlyph.Visibility = Visibility.Collapsed;
                    return;
            }

            IconGlyph.Text = glyph;
            if (TryFindResource(brushKey) is Brush brush)
            {
                IconGlyph.Foreground = brush;
            }

            AutomationProperties.SetName(IconGlyph, name);
            IconGlyph.Visibility = Visibility.Visible;
        }

        private void BuildButtons(MessageBoxButton button, MessageBoxResult defaultResult)
        {
            var buttons = button switch
            {
                MessageBoxButton.OKCancel => new[]
                {
                    (MessageBoxResult.OK, "OK"),
                    (MessageBoxResult.Cancel, "Cancel"),
                },
                MessageBoxButton.YesNo => new[]
                {
                    (MessageBoxResult.Yes, "Yes"),
                    (MessageBoxResult.No, "No"),
                },
                MessageBoxButton.YesNoCancel => new[]
                {
                    (MessageBoxResult.Yes, "Yes"),
                    (MessageBoxResult.No, "No"),
                    (MessageBoxResult.Cancel, "Cancel"),
                },
                MessageBoxButton.AbortRetryIgnore => new[]
                {
                    (MessageBoxResult.Abort, "Abort"),
                    (MessageBoxResult.Retry, "Retry"),
                    (MessageBoxResult.Ignore, "Ignore"),
                },
                MessageBoxButton.RetryCancel => new[]
                {
                    (MessageBoxResult.Retry, "Retry"),
                    (MessageBoxResult.Cancel, "Cancel"),
                },
                MessageBoxButton.CancelTryContinue => new[]
                {
                    (MessageBoxResult.Cancel, "Cancel"),
                    (MessageBoxResult.TryAgain, "Try Again"),
                    (MessageBoxResult.Continue, "Continue"),
                },
                _ => new[]
                {
                    (MessageBoxResult.OK, "OK"),
                },
            };

            // Closing via Esc or the title-bar close button returns the cancel-equivalent, mirroring
            // the native message box: Cancel if present, otherwise No, otherwise the last button.
            _cancelResult = buttons.Any(b => b.Item1 == MessageBoxResult.Cancel) ? MessageBoxResult.Cancel
                : buttons.Any(b => b.Item1 == MessageBoxResult.No) ? MessageBoxResult.No
                : buttons[buttons.Length - 1].Item1;

            bool hasExplicitDefault = defaultResult != MessageBoxResult.None
                && buttons.Any(b => b.Item1 == defaultResult);

            Button? defaultButton = null;

            for (int i = 0; i < buttons.Length; i++)
            {
                (MessageBoxResult result, string label) = buttons[i];

                var uiButton = new Button
                {
                    Content = label,
                    MinWidth = 80,
                    Margin = new Thickness(i == 0 ? 0 : 8, 0, 0, 0),
                    IsDefault = hasExplicitDefault ? result == defaultResult : i == 0,
                    IsCancel = result == _cancelResult,
                };
                uiButton.Click += (_, _) =>
                {
                    _result = result;
                    Close();
                };

                if (uiButton.IsDefault)
                {
                    defaultButton = uiButton;
                }

                ButtonPanel.Children.Add(uiButton);
            }

            Loaded += (_, _) => defaultButton?.Focus();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            if (_result == MessageBoxResult.None)
            {
                _result = _cancelResult;
            }

            base.OnClosed(e);
        }
    }
}
