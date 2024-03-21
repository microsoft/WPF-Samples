using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.ViewModels;
using Win11ThemeGallery.Views;
using Win11ThemeGallery.Views.Samples;
using Win11ThemeGallery.ViewModels.Samples;

namespace Win11ThemeGallery;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            
            services.AddTransient<DashboardPage>();
            services.AddTransient<DashboardPageViewModel>();

            services.AddTransient<ButtonPage>();
            services.AddTransient<ButtonPageViewModel>();
            services.AddTransient<CheckBoxPage>();
            services.AddTransient<CheckBoxPageViewModel>();
            services.AddTransient<ComboBoxPage>();
            services.AddTransient<ComboBoxPageViewModel>();
            services.AddTransient<RadioButtonPage>();
            services.AddTransient<RadioButtonPageViewModel>();
            services.AddTransient<SliderPage>();
            services.AddTransient<SliderPageViewModel>();
            services.AddTransient<CalendarPage>();
            services.AddTransient<CalendarPageViewModel>();
            services.AddTransient<DatePickerPage>();
            services.AddTransient<DatePickerPageViewModel>();
            services.AddTransient<TabControlPage>();
            services.AddTransient<TabControlPageViewModel>();
            services.AddTransient<ProgressBarPage>();
            services.AddTransient<ProgressBarPageViewModel>();
            services.AddTransient<MenuPage>();
            services.AddTransient<MenuPageViewModel>();
            services.AddTransient<ToolTipPage>();
            services.AddTransient<ToolTipPageViewModel>();
            services.AddTransient<CanvasPage>();
            services.AddTransient<CanvasPageViewModel>();
            services.AddTransient<ExpanderPage>();
            services.AddTransient<ExpanderPageViewModel>();
            services.AddTransient<ImagePage>();
            services.AddTransient<ImagePageViewModel>();
            services.AddTransient<DataGridPage>();
            services.AddTransient<DataGridPageViewModel>();
            services.AddTransient<ListBoxPage>();
            services.AddTransient<ListBoxPageViewModel>();
            services.AddTransient<ListViewPage>();
            services.AddTransient<ListViewPageViewModel>();
            services.AddTransient<TreeViewPage>();
            services.AddTransient<TreeViewPageViewModel>();
            services.AddTransient<LabelPage>();
            services.AddTransient<LabelPageViewModel>();
            services.AddTransient<TextBoxPage>();
            services.AddTransient<TextBoxPageViewModel>();
            services.AddTransient<TextBlockPage>();
            services.AddTransient<TextBlockPageViewModel>();
            services.AddTransient<RichTextEditPage>();
            services.AddTransient<RichTextEditPageViewModel>();
            services.AddTransient<PasswordBoxPage>();
            services.AddTransient<PasswordBoxPageViewModel>();

            services.AddTransient<LayoutPage>();
            services.AddTransient<LayoutPageViewModel>();
            services.AddTransient<BasicInputPage>();
            services.AddTransient<BasicInputPageViewModel>();
            services.AddTransient<CollectionsPage>();
            services.AddTransient<CollectionsPageViewModel>();
            services.AddTransient<MediaPage>();
            services.AddTransient<MediaPageViewModel>();
            services.AddTransient<NavigationPage>();
            services.AddTransient<NavigationPageViewModel>();
            services.AddTransient<TextPage>();
            services.AddTransient<TextPageViewModel>();
            services.AddTransient<DateAndTimePage>();
            services.AddTransient<DateAndTimePageViewModel>();
            services.AddTransient<StatusAndInfoPage>();
            services.AddTransient<StatusAndInfoPageViewModel>();
            services.AddTransient<SamplesPage>();
            services.AddTransient<SamplesPageViewModel>();

            services.AddTransient<UserDashboardPage>();
            services.AddTransient<UserDashboardPageViewModel>();

            services.AddSingleton<SettingsPage>();
            services.AddSingleton<SettingsPageViewModel>();
        }).Build();


    [STAThread]
    public static void Main()
    {
        _host.Start();

        App app = new();
        app.InitializeComponent();
        app.MainWindow = _host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }
}

