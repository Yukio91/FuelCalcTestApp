using SQLite;

namespace FuelCalcTestApp.Models;

[Table("Routes")]
public class Route
{
    [PrimaryKey, AutoIncrement]
    public int RouteId { get; set; }
    public string DeparturePoint { get; set; } = "";
    public string ArrivalPoint { get; set; } = "";
    public decimal Distance { get; set; }          // км
}
