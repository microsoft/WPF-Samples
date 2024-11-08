using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGallery.Models;
using WPFGallery.Navigation;

namespace WPFGallery.ViewModels
{
    public partial class WhatsNewPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "What's new in WPF";

        [ObservableProperty]
        private string _pageDescription = "Lists all the new features, enhancements and APIs introduced in WPF for .NET 9";

        [ObservableProperty]
        private string _accentColorXamlCode = _accentColorBrushApiXamlUsage;

        [ObservableProperty]
        private string _hyphenBasedLigatureXamlCode = _hyphenBasedLiagatureXamlUsage;

        private const string _accentColorBrushApiXamlUsage = """
            <StackPanel Orientation="Horizontal" Height="50">
                <StackPanel.Resources>
                    <Style TargetType="Border">
                        <Setter Property="Height" Value="50" />
                        <Setter Property="Width" Value="30" />
                    </Style>
                </StackPanel.Resources>
                <Border CornerRadius="2 0 0 2" Background="{x:Static SystemColors.AccentColorDark3Brush}" />
                <Border Background="{x:Static SystemColors.AccentColorDark2Brush}" />
                <Border Background="{x:Static SystemColors.AccentColorDark1Brush}" />
                <Border Background="{x:Static SystemColors.AccentColorBrush}" />
                <Border Background="{x:Static SystemColors.AccentColorLight1Brush}" />
                <Border Background="{x:Static SystemColors.AccentColorLight2Brush}" />
                <Border CornerRadius="0 2 2 0" Background="{x:Static SystemColors.AccentColorLight3Brush}" />
            </StackPanel>
            """;

        private const string _hyphenBasedLiagatureXamlUsage = """
            <StackPanel Orientation="Horizontal">
                <TextBlock Margin="0 0 16 0" FontFamily="Cascadia Code" Text="-->" />
                <TextBlock Margin="0 0 16 0" FontFamily="Cascadia Code" Text="&lt;!--" />
                <TextBlock Margin="0 0 16 0" FontFamily="Cascadia Code" Text="&lt;--" />
            </StackPanel>
            """;
    }
}
