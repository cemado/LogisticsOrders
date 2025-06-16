using LogisticsOrders.Domain.ValueObjects;

namespace LogisticsOrders.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string Client { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public GeoPoint Origin { get; set; } = default!;
    public GeoPoint Destination { get; set; } = default!;
    public double DistanceKm { get; set; }
    public decimal EstimatedCost { get; set; }
    public DateTime CreatedAt { get; set; }
}