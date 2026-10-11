using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FuelCalcTestApp.Services;
using FuelCalcTestApp.ViewModels;
using FuelCalcTestApp.Views;

namespace FuelCalcTestApp;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var db = new Database();
            var window = new MainWindow();

            window.DataContext = new MainViewModel(db, new DialogService(window));
            desktop.MainWindow = window;

            desktop.Exit += (_, _) => db.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
