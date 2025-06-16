using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Services;
using LogisticsOrders.Domain.ValueObjects;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Application.Interfaces;

namespace LogisticsOrders.Application.Orders;

public class EditOrderHandler
{
    private readonly IOrderRepository _repo;
    private readonly IExternalGeoApiService _geoApiService;
    private readonly IOrderAuditRepository? _auditRepo;

    public EditOrderHandler(
        IOrderRepository repo,
        IExternalGeoApiService geoApiService,
        IOrderAuditRepository? auditRepo = null
    )
    {
        _repo = repo;
        _geoApiService = geoApiService;
        _auditRepo = auditRepo;
    }

    public async Task EditAsync(int id, CreateOrderCommand cmd, string? userName = null)
    {
        var order = _repo.GetById(id);
        if (order == null)
            throw new InvalidOperationException("Orden no encontrada.");

        // Validar dirección de origen
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

        // Validación de producto
        var validProducts = new[] { "Producto1", "Producto2", "Producto3" };
        if (!OrderBusinessValidator.IsProductValid(cmd.Product, validProducts))
            throw new InvalidOperationException("El producto no es válido.");

        // Validación de duplicados (excluyendo la orden actual)
        var existingOrders = _repo.GetByClientWithFilters(cmd.Client, cmd.Product, DateTime.Today, DateTime.Today)
            .Where(o => o.Id != id);
        var updatedOrder = new Order
        {
            Client = cmd.Client,
            Product = cmd.Product,
            Quantity = cmd.Quantity,
            Origin = origin,
            Destination = dest,
            DistanceKm = distance,
            EstimatedCost = cost,
            CreatedAt = order.CreatedAt // Mantén la fecha original
        };
        if (OrderBusinessValidator.IsDuplicateOrder(updatedOrder, existingOrders))
            throw new InvalidOperationException("Ya existe una orden igual para este cliente, producto y ruta hoy.");

        // Actualiza los campos
        order.Client = cmd.Client;
        order.Product = cmd.Product;
        order.Quantity = cmd.Quantity;
        order.Origin = origin;
        order.Destination = dest;
        order.DistanceKm = distance;
        order.EstimatedCost = cost;

        _repo.Update(order);

        // Auditoría
        _auditRepo?.Add(new OrderAudit
        {
            OrderId = order.Id,
            Action = "Edición",
            User = userName ?? "Sistema",
            Timestamp = DateTime.UtcNow,
            Details = "Orden editada"
        });
    }
}