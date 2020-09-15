using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly GetCart getCart;

        public CartViewComponent(GetCart getCart)
        {
            this.getCart = getCart;
        }

        public IViewComponentResult Invoke(string view = "Default")
        {
            if (view == "Small")
            {
                var totalValue = getCart.Do().Sum(x => x.RealValue * x.Qty);
                return View(view, $"{totalValue} ₺");
            }
            //We can use this if we want to use different UI but same data
            return View(view, getCart.Do());
        }
    }
}
