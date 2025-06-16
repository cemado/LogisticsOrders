using System.Collections.Generic;
using LogisticsOrders.Domain.Entities;

namespace LogisticsOrders.Application.Interfaces;

public interface IOrderService
{
    IEnumerable<Order> GetOrders();
    Order? GetOrder(int id);
    void CreateOrder(Order order);
    void UpdateOrder(Order order);
    void DeleteOrder(int id);
}