using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Orders;
using Shop.Database;
using Shop.Domain.Infrastructure;

namespace Shop.UI.Pages
{
    public class OrderModel : PageModel
    {
        private readonly IOrderManager _orderManager;

        public OrderModel(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public GetOrder.Response Order { get; set; }

        public void OnGet(string reference)
        {
            Order = new GetOrder(_orderManager).Do(reference);
        }
    }
}
