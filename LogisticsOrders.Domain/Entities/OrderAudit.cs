using System;

namespace LogisticsOrders.Domain.Entities;

public class OrderAudit
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Action { get; set; } = string.Empty; // "Creación", "Edición", "Eliminación"
    public string User { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Details { get; set; }
}