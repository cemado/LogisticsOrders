using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Services;
using LogisticsOrders.Domain.ValueObjects;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Application.Interfaces;

namespace LogisticsOrders.Application.Orders;

public class CreateOrderHandler
{
    private readonly IOrderRepository _repo;
    private readonly IExternalGeoApiService _geoApiService;
    private readonly IOrderAuditRepository? _auditRepo;

    public CreateOrderHandler(
        IOrderRepository repo,
        IExternalGeoApiService geoApiService,
        IOrderAuditRepository? auditRepo = null
    )
    {
        _repo = repo;
        _geoApiService = geoApiService;
        _auditRepo = auditRepo;
    }

    public async Task<int> Handle(CreateOrderCommand cmd, string? userName = null)
    {
        var originAddress = $"{cmd.OriginLat},{cmd.OriginLng}";
        var isValid = await _geoApiService.ValidateAddressAsync(originAddress);
        if (!isValid)
            throw new InvalidOperationException("La dirección de origen no es válida.");

        var origin = new GeoPoint(cmd.OriginLat, cmd.OriginLng);
        var dest = new GeoPoint(cmd.DestLat, cmd.DestLng);
        var distance = DistanceCalculator.HaversineKm(origin, dest);

        if (distance < 1 || distance > 1000)
            throw new InvalidOperationException("La distancia debe estar entre 1 y 1000 km.");

        var cost = OrderCostCalculator.CalculateCost(distance);

        var validProducts = new[] { "Producto1", "Producto2", "Producto3" };
        if (!OrderBusinessValidator.IsProductValid(cmd.Product, validProducts))
            throw new InvalidOperationException("El producto no es válido.");

        var existingOrders = _repo.GetByClientWithFilters(cmd.Client, cmd.Product, DateTime.Today, DateTime.Today);
        var newOrder = new Order
        {
            Client = cmd.Client,
            Product = cmd.Product,
            Quantity = cmd.Quantity,
            Origin = origin,
            Destination = dest,
            DistanceKm = distance,
            EstimatedCost = cost,
            CreatedAt = DateTime.UtcNow
        };
        if (OrderBusinessValidator.IsDuplicateOrder(newOrder, existingOrders))
            throw new InvalidOperationException("Ya existe una orden igual para este cliente, producto y ruta hoy.");

        _repo.Add(newOrder);

        if (_auditRepo != null)
        {
            _auditRepo.Add(new OrderAudit
            {
                OrderId = newOrder.Id,
                Action = "Creación",
                User = userName ?? "Sistema",
                Timestamp = DateTime.UtcNow,
                Details = "Orden creada"
            });
        }

        return newOrder.Id;
    }
}