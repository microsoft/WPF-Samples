// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows.Markup;

namespace WPFGallery.Controls;

/// <summary>
/// A control that displays an example of a control
/// </summary>

[ContentProperty(nameof(ExampleContent))]
public class ControlExample : Control
{
    static ControlExample()
    {
        CommandManager.RegisterClassCommandBinding(typeof(ControlExample), new CommandBinding(ApplicationCommands.Copy, Copy_SourceCode));
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

    private static void Copy_SourceCode(object sender, RoutedEventArgs e)
    {
        if (sender is ControlExample controlExample)
        {
            if(!string.IsNullOrEmpty(controlExample.XamlCode))
            {
                try
                {
                    switch (((ExecutedRoutedEventArgs)e).Parameter.ToString())
                    {
                        case "Copy_XamlCode":
                            Clipboard.SetText(controlExample.XamlCode);
                            break;
                        case "Copy_CsharpCode":
                            Clipboard.SetText(controlExample.CsharpCode);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error copying to clipboard: " + ex.Message);
                }
            }
        }
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
}

