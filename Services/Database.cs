using System;
using System.Collections.Generic;
using System.IO;
using FuelCalcTestApp.Models;
using SQLite;

namespace FuelCalcTestApp.Services;

public class Database : IDisposable
{
    private const string _databaseName = "fuel.db";

    private readonly SQLiteConnection _db;

    public Database()
    {
        var path = Path.Combine(AppContext.BaseDirectory, _databaseName);

        _db = new SQLiteConnection(path);

        _db.CreateTable<Route>();
        _db.CreateTable<Car>();
    }

    #region Routes

    public List<Route> GetRoutes() =>
        _db.Query<Route>("SELECT RouteId, DeparturePoint, ArrivalPoint, Distance FROM Routes");

    public List<string> GetDepartures() =>
        _db.QueryScalars<string>("SELECT DISTINCT DeparturePoint FROM Routes ORDER BY DeparturePoint");

    public List<Route> GetRoutesFrom(string from) =>
        _db.Query<Route>(
            "SELECT RouteId, DeparturePoint, ArrivalPoint, Distance FROM Routes " +
            "WHERE DeparturePoint = ? ORDER BY ArrivalPoint", from);

    public Route? GetRoute(string from, string to) =>
        _db.FindWithQuery<Route>(
            "SELECT RouteId, DeparturePoint, ArrivalPoint, Distance FROM Routes " +
            "WHERE DeparturePoint = ? AND ArrivalPoint = ? LIMIT 1", from, to);

    public void AddRoute(string from, string to, decimal distance) =>
        _db.Execute(
            "INSERT INTO Routes (DeparturePoint, ArrivalPoint, Distance) VALUES (?, ?, ?)",
            from, to, distance);

    #endregion Routes

    #region Cars

    public IEnumerable<Car> GetCars() =>
        _db.DeferredQuery<Car>("SELECT CarId, CarBrand, FuelPerKm FROM Cars");

    public void AddCar(string brand, decimal fuelPerKm) =>
        _db.Insert(new Car { CarBrand = brand, FuelPerKm = fuelPerKm });

    #endregion Cars

    #region IDisposable

    private bool _disposed;
    public void Dispose()
    {
        if (_disposed)
            return;

        _db.Dispose();

        _disposed = true;
    }

    #endregion IDisposable
}
