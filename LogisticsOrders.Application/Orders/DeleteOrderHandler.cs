using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Domain.Entities;

namespace LogisticsOrders.Application.Orders;

public class DeleteOrderHandler
{
    private readonly IOrderRepository _repo;
    private readonly IOrderAuditRepository? _auditRepo;

    public DeleteOrderHandler(
        IOrderRepository repo,
        IOrderAuditRepository? auditRepo = null
    )
    {
        _repo = repo;
        _auditRepo = auditRepo;
    }

    public void Delete(int id, string? userName = null)
    {
        var order = _repo.GetById(id);
        if (order == null)
            throw new InvalidOperationException("Orden no encontrada.");

        _repo.Delete(id);

        // Auditoría
        _auditRepo?.Add(new OrderAudit
        {
            OrderId = id,
            Action = "Eliminación",
            User = userName ?? "Sistema",
            Timestamp = DateTime.UtcNow,
            Details = "Orden eliminada"
        });
    }
}