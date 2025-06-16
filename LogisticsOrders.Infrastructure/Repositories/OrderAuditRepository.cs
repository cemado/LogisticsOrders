using System.Collections.Generic;
using System.Linq;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Infrastructure.Data;

namespace LogisticsOrders.Infrastructure.Repositories;

public class OrderAuditRepository : IOrderAuditRepository
{
    private readonly LogisticsDbContext _context;

    public OrderAuditRepository(LogisticsDbContext context)
    {
        _context = context;
    }

    public void Add(OrderAudit audit)
    {
        _context.Set<OrderAudit>().Add(audit);
        _context.SaveChanges();
    }

    public IEnumerable<OrderAudit> GetByOrderId(int orderId)
    {
        return _context.Set<OrderAudit>().Where(a => a.OrderId == orderId).ToList();
    }

    public void AddOrderCreationAudit(Order order, string userName)
    {
        Add(new OrderAudit
        {
            OrderId = order.Id,
            Action = "Creación",
            User = userName ?? "Sistema",
            Timestamp = DateTime.UtcNow,
            Details = "Orden creada"
        });
    }
}