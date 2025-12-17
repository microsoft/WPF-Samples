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
        private string _pageDescription = "Discover all the new features, enhancements and APIs introduced in WPF";

        [ObservableProperty]
        private string _accentColorXamlCode = _accentColorBrushApiXamlUsage;

        [ObservableProperty]
        private string _hyphenBasedLigatureXamlCode = _hyphenBasedLiagatureXamlUsage;

        [ObservableProperty]
        private string _gridShorthandSyntaxXamlCode = _gridShorthandSyntaxXamlUsage;

        private readonly INavigationService _navigationService;

        public WhatsNewPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Navigate(object pageType)
        {
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }

        private const string _accentColorBrushApiXamlUsage = """
            <StackPanel Orientation="Horizontal" Height="50">
                <StackPanel.Resources>
                    <Style TargetType="Border">
                        <Setter Property="Height" Value="50" />
                        <Setter Property="Width" Value="30" />
                    </Style>
                </StackPanel.Resources>
                <Border CornerRadius="2 0 0 2" Background="{DynamicResource {x:Static SystemColors.AccentColorDark3BrushKey}}" />
                <Border Background="{DynamicResource {x:Static SystemColors.AccentColorDark2BrushKey}}" />
                <Border Background="{DynamicResource {x:Static SystemColors.AccentColorDark1BrushKey}}" />
                <Border Background="{DynamicResource {x:Static SystemColors.AccentColorBrushKey}}" />
                <Border Background="{DynamicResource {x:Static SystemColors.AccentColorLight1BrushKey}}" />
                <Border Background="{DynamicResource {x:Static SystemColors.AccentColorLight2BrushKey}}" />
                <Border CornerRadius="0 2 2 0" Background="{DynamicResource {x:Static SystemColors.AccentColorLight3BrushKey}}" />
            </StackPanel>
            """;

        private const string _hyphenBasedLiagatureXamlUsage = """
            <StackPanel Orientation="Horizontal">
                <TextBlock Margin="0 0 16 0" FontFamily="Cascadia Code" Text="-->" />
                <TextBlock Margin="0 0 16 0" FontFamily="Cascadia Code" Text="&lt;!--" />
                <TextBlock Margin="0 0 16 0" FontFamily="Cascadia Code" Text="&lt;--" />
            </StackPanel>
            """;

        private const string _gridShorthandSyntaxXamlUsage = """
            <Grid MaxWidth="500" RowDefinitions="Auto,Auto,Auto" ColumnDefinitions="Auto 80 *" HorizontalAlignment="Left">
                <TextBlock Grid.Row="0" Grid.Column="0" FontWeight="Bold" Margin="0 0 10 0">Sl. No.</TextBlock>
                <TextBlock Grid.Row="0" Grid.Column="1" FontWeight="Bold">Name</TextBlock>
                <TextBlock Grid.Row="0" Grid.Column="2" FontWeight="Bold">Description</TextBlock>
                <TextBlock Grid.Row="1" Grid.Column="0">1</TextBlock>
                <TextBlock Grid.Row="1" Grid.Column="1">Rectangle</TextBlock>
                <TextBlock Grid.Row="1" Grid.Column="2" TextWrapping="Wrap">Quadrilateral where all the adjacent sides form a right angle.</TextBlock>
                <TextBlock Grid.Row="2" Grid.Column="0">2</TextBlock>
                <TextBlock Grid.Row="2" Grid.Column="1">Circle</TextBlock>
                <TextBlock Grid.Row="2" Grid.Column="2" TextWrapping="Wrap">Set of all points that are equidistant from a fixed point.</TextBlock>
            </Grid>
            """;
    }
}
