using Shop.Domain.Infrastructure;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    [Service]
    public class RemoveFromCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly IStockManager _stockManager;

        public RemoveFromCart(ISessionManager sessionManager, IStockManager stockManager)
        {
            _sessionManager = sessionManager;
            _stockManager = stockManager;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
            public bool All { get; set; }

        }
        public async Task<bool> Do(Request request)
        {
            _sessionManager.RemoveProduct(request.StockId, request.Qty, request.All);

            var stockOnHold = _stockManager.GetStocksOnHoldByIdWithSessionId(request.StockId, _sessionManager.GetId());

            var stock = _stockManager.GetStock(request.StockId);

            if (request.All)
            {
                stock.Qty += stockOnHold.Qty;
                stockOnHold.Qty = 0;
            }
            else
            {
                stock.Qty += request.Qty;
                stockOnHold.Qty -= request.Qty;
            }

            if (stockOnHold.Qty <= 0)
            {
                await _stockManager.RemoveStockFromHold(stockOnHold);
            }

            await _stockManager.UpdateStock(stock);

            await _stockManager.UpdateStockOnHold(stockOnHold);

            return true;
        }
    }
}
