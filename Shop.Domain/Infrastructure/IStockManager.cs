using Shop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IStockManager
    {
        Task<int> CreateStock(Stock stock);
        Task<int> DeleteStock(int id);
        Task<int> UpdateStockRange(List<Stock> stocks);
        Task<int> UpdateStock(Stock stock);



        Stock GetStockWithProduct(int stockId);
        Stock GetStock(int stockId);
        bool EnoughStock(int stockId, int qty);
        Task RetrieveExpiredStockOnHold();
        Task PutStockOnHold(int stockId, int qty, string sessionId);
        Task<bool> RemoveStockFromHold(string sessionId);
        Task<int> RemoveStockFromHold(StocksOnHold stockOnHold);
        Task<int> UpdateStockOnHold(StocksOnHold stockOnHold);
        StocksOnHold GetStocksOnHoldByIdWithSessionId(int stockId, string sessionId);

    }
}
