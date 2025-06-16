using LogisticsOrders.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsOrders.Domain.Services;

public static class OrderBusinessValidator
{
    public static bool IsProductValid(string product, IEnumerable<string> validProducts)
        => validProducts.Contains(product);

    public static bool IsDuplicateOrder(Order newOrder, IEnumerable<Order> existingOrders)
        => existingOrders.Any(o =>
            o.Client == newOrder.Client &&
            o.Product == newOrder.Product &&
            o.Origin.Equals(newOrder.Origin) &&
            o.Destination.Equals(newOrder.Destination) &&
            o.CreatedAt.Date == newOrder.CreatedAt.Date);
}