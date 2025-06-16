using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using System.Collections.Generic;
using System.Linq;
using X.PagedList.Extensions;

namespace LogisticsOrders.API.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public IndexModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string? ClientName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public IPagedList<OrderViewModel>? Orders { get; set; }

        public void OnGet()
        {
            var query = _orderRepository.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(ClientName))
                query = query.Where(o => o.Client.Contains(ClientName));

            if (!string.IsNullOrWhiteSpace(Product))
                query = query.Where(o => o.Product.Contains(Product));

            Orders = query
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    CustomerName = o.Client,
                    OrderDate = o.CreatedAt,
                    Total = o.EstimatedCost
                })
                .ToPagedList(PageNumber, 10);
        }

        public class OrderViewModel
        {
            public int Id { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public DateTime OrderDate { get; set; }
            public decimal Total { get; set; }
        }
    }
}