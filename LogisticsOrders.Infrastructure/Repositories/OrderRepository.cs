using System;
using System.Collections.Generic;
using System.Linq;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Infrastructure.Data;

namespace LogisticsOrders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly LogisticsDbContext _context;

    public OrderRepository(LogisticsDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Order> GetAll()
    {
        return _context.Orders.ToList();
    }

    public Order? GetById(int id)
    {
        return _context.Orders.Find(id);
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Order> GetByClient(string clientName)
    {
        return _context.Orders
            .Where(o => o.Client.Equals(clientName, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IEnumerable<Order> GetByClientWithFilters(string clientName, string? product, DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.Orders.AsQueryable();

        query = query.Where(o => o.Client.Equals(clientName, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(product))
            query = query.Where(o => o.Product.Contains(product));

        if (fromDate.HasValue)
            query = query.Where(o => o.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(o => o.CreatedAt <= toDate.Value);

        return query.ToList();
    }
}