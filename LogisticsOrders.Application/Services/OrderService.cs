using System.Collections.Generic;
using LogisticsOrders.Application.Interfaces;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Domain.Services;

namespace LogisticsOrders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IEnumerable<Order> GetOrders()
    {
        return _orderRepository.GetAll();
    }

    public Order? GetOrder(int id)
    {
        return _orderRepository.GetById(id);
    }

    public void CreateOrder(Order order)
    {
        var validProducts = new[] { "Producto1", "Producto2", "Producto3" };
        if (!OrderBusinessValidator.IsProductValid(order.Product, validProducts))
            throw new InvalidOperationException("El producto no es válido.");

        var existingOrders = _orderRepository.GetByClientWithFilters(order.Client, order.Product, DateTime.Today, DateTime.Today);
        if (OrderBusinessValidator.IsDuplicateOrder(order, existingOrders))
            throw new InvalidOperationException("Ya existe una orden igual para este cliente, producto y ruta hoy.");

        _orderRepository.Add(order);
    }

    public void UpdateOrder(Order order)
    {
        _orderRepository.Update(order);
    }

    public void DeleteOrder(int id)
    {
        _orderRepository.Delete(id);
    }
}