using Avalonia.Controls;
using FuelCalcTestApp.ViewModels;
using FuelCalcTestApp.Views;
using System.Threading.Tasks;

namespace FuelCalcTestApp.Services;

public class DialogService : IDialogService
{
    private readonly Window _owner;
    public DialogService(Window owner) => _owner = owner;

    public Task ShowAddDataAsync(AddDataViewModel vm) =>
        new AddDataWindow { DataContext = vm }.ShowDialog(_owner);
}
