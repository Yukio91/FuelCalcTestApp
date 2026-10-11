using FuelCalcTestApp.ViewModels;
using System.Threading.Tasks;

namespace FuelCalcTestApp.Services;

public interface IDialogService
{
    Task ShowAddDataAsync(AddDataViewModel vm);
}
