// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO;
using System.Windows.Automation.Peers;
using System.Windows.Automation;

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

    public static readonly DependencyProperty CSharpCodeProperty = DependencyProperty.Register(
        nameof(CSharpCode),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty CSharpCodeSourceProperty = DependencyProperty.Register(
        nameof(CSharpCodeSource),
        typeof(Uri),
        typeof(ControlExample),
        new PropertyMetadata(
            null,
            static (o, args) => ((ControlExample)o).OnCSharpCodeSourceChanged((Uri)args.NewValue)
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

    public string? CSharpCode
    {
        get => (string)GetValue(CSharpCodeProperty);
        set => SetValue(CSharpCodeProperty, value);
    }

    public Uri? CSharpCodeSource
    {
        get => (Uri)GetValue(CSharpCodeSourceProperty);
        set => SetValue(CSharpCodeSourceProperty, value);
    }

    private void OnXamlCodeSourceChanged(Uri uri)
    {
        XamlCode = LoadResource(uri);
    }

    private void OnCSharpCodeSourceChanged(Uri uri)
    {
        CSharpCode = LoadResource(uri);
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
                            AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement((Button)e.OriginalSource);
                            peer.RaiseNotificationEvent(
                                AutomationNotificationKind.Other,
                                AutomationNotificationProcessing.ImportantMostRecent,
                                "Source Code Copied",
                                "ButtonClickedActivity"
                            );
                            break;
                        case "Copy_CSharpCode":
                            Clipboard.SetText(controlExample.CSharpCode);
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

