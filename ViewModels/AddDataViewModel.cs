using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuelCalcTestApp.Services;

namespace FuelCalcTestApp.ViewModels;

public partial class AddDataViewModel : ObservableObject
{
    private readonly Database _db;
    public AddDataViewModel(Database db) => _db = db;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddRouteCommand))]
    private string? _routeFrom;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddRouteCommand))]
    private string? _routeTo;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddRouteCommand))]
    private decimal? _routeDistance;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCarCommand))]
    private string? _carBrand;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCarCommand))]
    private decimal? _carFuelPerKm;

    [ObservableProperty]
    private string _status = "";

    private bool CanAddRoute() =>
        !string.IsNullOrWhiteSpace(RouteFrom) &&
        !string.IsNullOrWhiteSpace(RouteTo) &&
        RouteDistance is > 0;

    private bool CanAddCar() =>
        !string.IsNullOrWhiteSpace(CarBrand) && CarFuelPerKm is > 0;

    [RelayCommand(CanExecute = nameof(CanAddRoute))]
    private void AddRoute()
    {
        _db.AddRoute(RouteFrom!.Trim(), RouteTo!.Trim(), RouteDistance!.Value);
        
        Status = $"Маршрут «{RouteFrom} → {RouteTo}» добавлен";
        
        RouteFrom = RouteTo = null;
        RouteDistance = null;
    }

    [RelayCommand(CanExecute = nameof(CanAddCar))]
    private void AddCar()
    {
        _db.AddCar(CarBrand!.Trim(), CarFuelPerKm!.Value);
        
        Status = $"Марка «{CarBrand}» добавлена";

        CarBrand = null;
        CarFuelPerKm = null;
    }
}
