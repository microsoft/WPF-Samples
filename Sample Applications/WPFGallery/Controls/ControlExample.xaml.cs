// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace WPFGallery.Controls;

[ContentProperty(nameof(ExampleContent))]
public class ControlExample : Control
{
    static ControlExample()
    {
        if (DefaultStyleKeyProperty.GetMetadata(typeof(ControlExample)).DefaultValue == null)
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlExample), new FrameworkPropertyMetadata(typeof(ControlExample)));
        }
    }
    
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var myButton = GetTemplateChild("MyButton") as Button;
        var myTextBox = GetTemplateChild("XamlCode") as TextBox;

        if (myButton != null)
        {
            myButton.Click += (s, e) =>
            {
                if (myTextBox != null)
                {
                    Clipboard.SetText(myTextBox.Text);
                }

                var copyBlock = GetTemplateChild("CopyTextBlock") as TextBlock;
            };
        }
    }

    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
        nameof(HeaderText),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty ExampleContentProperty = DependencyProperty.Register(
        nameof(ExampleContent),
        typeof(object),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty XamlCodeProperty = DependencyProperty.Register(
        nameof(XamlCode),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty XamlCodeSourceProperty = DependencyProperty.Register(
        nameof(XamlCodeSource),
        typeof(Uri),
        typeof(ControlExample),
        new PropertyMetadata(
            null,
            static (o, args) => ((ControlExample)o).OnXamlCodeSourceChanged((Uri)args.NewValue)
        )
    );

    public static readonly DependencyProperty CsharpCodeProperty = DependencyProperty.Register(
        nameof(CsharpCode),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty CsharpCodeSourceProperty = DependencyProperty.Register(
        nameof(CsharpCodeSource),
        typeof(Uri),
        typeof(ControlExample),
        new PropertyMetadata(
            null,
            static (o, args) => ((ControlExample)o).OnCsharpCodeSourceChanged((Uri)args.NewValue)
        )
    );

    public string? HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public object? ExampleContent
    {
        get => GetValue(ExampleContentProperty);
        set => SetValue(ExampleContentProperty, value);
    }

    public string? XamlCode
    {
        get => (string)GetValue(XamlCodeProperty);
        set => SetValue(XamlCodeProperty, value);
    }

    public Uri? XamlCodeSource
    {
        get => (Uri)GetValue(XamlCodeSourceProperty);
        set => SetValue(XamlCodeSourceProperty, value);
    }

    public string? CsharpCode
    {
        get => (string)GetValue(CsharpCodeProperty);
        set => SetValue(CsharpCodeProperty, value);
    }

    public Uri? CsharpCodeSource
    {
        get => (Uri)GetValue(CsharpCodeSourceProperty);
        set => SetValue(CsharpCodeSourceProperty, value);
    }

    private void OnXamlCodeSourceChanged(Uri uri)
    {
        XamlCode = LoadResource(uri);
    }

    private void OnCsharpCodeSourceChanged(Uri uri)
    {
        CsharpCode = LoadResource(uri);
    }

    private static string LoadResource(Uri uri)
    {
        try
        {
            if (Application.GetResourceStream(uri) is not { } steamInfo)
            {
                return String.Empty;
            }

            using StreamReader streamReader = new(steamInfo.Stream, Encoding.UTF8);

            return streamReader.ReadToEnd();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return e.ToString();
        }
    }

    private void CopyText_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Ok");
    }
}

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;

    public RelayCommand(Action<object> execute)
    {
        _execute = execute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}

public static class ButtonClickCommand
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
        "Command",
        typeof(ICommand),
        typeof(ButtonClickCommand),
        new PropertyMetadata(null, OnCommandPropertyChanged));

    public static void SetCommand(DependencyObject d, ICommand value)
    {
        d.SetValue(CommandProperty, value);
    }

    public static ICommand GetCommand(DependencyObject d)
    {
        return (ICommand)d.GetValue(CommandProperty);
    }

    private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Button button = d as Button;
        if (button != null)
        {
            button.Click += (s, args) =>
            {
                ICommand command = GetCommand(button);
                if (command != null)
                {
                    command.Execute(button.DataContext);
                }
            };
        }
    }
}

