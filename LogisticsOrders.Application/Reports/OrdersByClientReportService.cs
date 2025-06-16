using System.Collections.Generic;
using System.Linq;
using System;

using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;

namespace LogisticsOrders.Application.Reports;

public class OrdersByClientReportService
{
    private readonly IOrderRepository _orderRepository;

    // Inyecci�n del repositorio en el constructor
    public OrdersByClientReportService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IEnumerable<Order> GetOrdersByClient(string clientName)
    {
        // Uso del repositorio para obtener �rdenes filtradas
        return _orderRepository.GetByClient(clientName);
    }
}