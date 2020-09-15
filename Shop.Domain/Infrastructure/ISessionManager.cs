using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface ISessionManager
    {
        string GetId();
        void AddProduct(CartProduct cartProduct);
        void RemoveProduct(int stockId, int qty, bool all);
        IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customerInformation);
        CustomerInformation GetCustomerInformation();
    }
}
