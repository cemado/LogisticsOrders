using LogisticsOrders.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LogisticsOrders.Application.Orders;

public class CreateOrderCommand
{
    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public string Client { get; set; } = string.Empty;

    [Required(ErrorMessage = "El producto es obligatorio.")]
    public string Product { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Quantity { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitud de origen inválida.")]
    public double OriginLat { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitud de origen inválida.")]
    public double OriginLng { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitud de destino inválida.")]
    public double DestLat { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitud de destino inválida.")]
    public double DestLng { get; set; }
}
