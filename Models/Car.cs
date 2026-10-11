using SQLite;

namespace FuelCalcTestApp.Models;

[Table("Cars")]
public class Car
{
    [PrimaryKey, AutoIncrement]
    public int CarId { get; set; }
    public string CarBrand { get; set; } = "";
    public decimal FuelPerKm { get; set; }         // л/км: 1 [л/км] * 1 [км] = 1 [л]
}
