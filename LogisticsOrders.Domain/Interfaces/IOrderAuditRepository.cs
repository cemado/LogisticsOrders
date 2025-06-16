using System.Collections.Generic;
using LogisticsOrders.Domain.Entities;

namespace LogisticsOrders.Domain.Interfaces;

public interface IOrderAuditRepository
{
    void Add(OrderAudit audit);
    IEnumerable<OrderAudit> GetByOrderId(int orderId);
}