using System.Configuration;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WPFGallery.Navigation;
using WPFGallery.ViewModels;
using WPFGallery.Views;
using WPFGallery.ViewModels.Samples;

namespace WPFGallery;

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
            services.AddTransient<ColorsPage>();
            services.AddTransient<ColorsPageViewModel>();

            services.AddTransient<LayoutPage>();
            services.AddTransient<LayoutPageViewModel>();
            services.AddTransient<AllSamplesPage>();
            services.AddTransient<AllSamplesPageViewModel>();
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
            services.AddTransient<DesignGuidancePage>();
            services.AddTransient<DesignGuidancePageViewModel>();

            services.AddTransient<UserDashboardPage>();
            services.AddTransient<UserDashboardPageViewModel>();

            services.AddTransient<TypographyPage>();
            services.AddTransient<TypographyPageViewModel>();

            services.AddSingleton<IconsPage>();
            services.AddSingleton<IconsPageViewModel>();

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

