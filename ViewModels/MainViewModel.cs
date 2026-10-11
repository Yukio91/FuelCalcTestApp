using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuelCalcTestApp.Models;
using FuelCalcTestApp.Services;

namespace FuelCalcTestApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly Database _db;
    private readonly IDialogService _dialogs;

    public ObservableCollection<string> Departures { get; } = new();
    public ObservableCollection<string> Arrivals { get; } = new();
    public ObservableCollection<Car> Cars { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsArrivalEnabled))]
    private string? _selectedDeparture;

    [ObservableProperty]
    private string? _selectedArrival;

    [ObservableProperty]
    private Car? _selectedCar;

    [ObservableProperty]
    private decimal _distance;   // км, только чтение во View

    [ObservableProperty]
    private decimal _fuel;       // л, только чтение во View

    public bool IsArrivalEnabled => SelectedDeparture != null;

    public MainViewModel(Database db, IDialogService dialogs)
    {
        _db = db;
        _dialogs = dialogs;

        Load();
    }

    private void Load()
    {
        SelectedDeparture = null;
        SelectedCar = null;

        Departures.Clear();
        foreach (var p in _db.GetDepartures())
            Departures.Add(p);

        Cars.Clear();
        foreach (var c in _db.GetCars())
            Cars.Add(c);
    }

    partial void OnSelectedDepartureChanged(string? value)
    {
        Arrivals.Clear();
        SelectedArrival = null;

        if (value == null)
            return;

        var routes = _db.GetRoutes();
        foreach (var r in routes.Where(r => r.DeparturePoint == value))
            Arrivals.Add(r.ArrivalPoint);
    }

    partial void OnSelectedArrivalChanged(string? value)
    {
        if (value == null || SelectedDeparture == null)
        {
            Distance = 0;
            return;
        }

        Distance = _db.GetRoute(SelectedDeparture, value)?.Distance ?? 0;
    }

    partial void OnSelectedCarChanged(Car? value) => RecalcFuel();
    partial void OnDistanceChanged(decimal value) => RecalcFuel();

    private void RecalcFuel() =>
        Fuel = SelectedCar is { } car && Distance > 0
            ? Math.Round(Distance * car.FuelPerKm, 2)        // км * (л/км) = л
            : 0;

    [RelayCommand]
    private async Task OpenAddDataAsync()
    {
        await _dialogs.ShowAddDataAsync(new AddDataViewModel(_db));

        Load();
    }
}
