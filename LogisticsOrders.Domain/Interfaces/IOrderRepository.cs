using System;
using System.Collections.Generic;
using LogisticsOrders.Domain.Entities;

namespace LogisticsOrders.Domain.Interfaces;

public interface IOrderRepository
{
    IEnumerable<Order> GetAll();
    Order? GetById(int id);
    void Add(Order order);
    void Update(Order order);
    void Delete(int id);

    // Métodos adicionales útiles para filtros y paginación
    IEnumerable<Order> GetByClient(string clientName);
    IEnumerable<Order> GetByClientWithFilters(string clientName, string? product, DateTime? fromDate, DateTime? toDate);
}