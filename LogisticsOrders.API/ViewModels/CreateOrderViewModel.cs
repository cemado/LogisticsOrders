using System.ComponentModel.DataAnnotations;

namespace LogisticsOrders.API.ViewModels;

public class CreateOrderViewModel
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }
}