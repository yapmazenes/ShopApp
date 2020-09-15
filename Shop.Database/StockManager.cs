using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class StockManager : IStockManager
    {
        private readonly ApplicationDbContext _ctx;

        public StockManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> CreateStock(Stock stock)
        {
            _ctx.Stocks.Add(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteStock(int id)
        {
            var stock = _ctx.Stocks.FirstOrDefault(x => x.Id == id);
            _ctx.Stocks.Remove(stock);
            return _ctx.SaveChangesAsync();
        }

        public bool EnoughStock(int stockId, int qty)
        {
            return _ctx.Stocks.FirstOrDefault(x => x.Id == stockId).Qty >= qty;
        }


        public Stock GetStockWithProduct(int stockId)
        {
            return _ctx.Stocks.Include(x => x.Product).FirstOrDefault(x => x.Id == stockId);
        }

        public Stock GetStock(int stockId)
        {
            return _ctx.Stocks.FirstOrDefault(x => x.Id == stockId);
        }

        public Task PutStockOnHold(int stockId, int qty, string sessionId)
        {
            //database responsibility to update the stock

            //Begin transaction

            //update Stock set qty = qty + {0} where Id = {1}
            _ctx.Stocks.FirstOrDefault(x => x.Id == stockId).Qty -= qty;

            var stockOnHold = _ctx.StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToList();

            //select count(*) from StocksOnHold where StockId = {0} and sessionId = {1}
            if (stockOnHold.Any(x => x.StockId == stockId))
            {
                //update StockOnHold set qty = qty + {0} where StockId = {1} and sessionId = {2}
                stockOnHold.Find(x => x.StockId == stockId).Qty += qty;
            }
            else
            {
                //insert into StockOnHold (StockId, SessionId, Qty, ExpiryDate) 
                // values ({0}, {1}, {2}, {3})

                _ctx.StocksOnHold.Add(new StocksOnHold
                {
                    StockId = stockId,
                    SessionId = sessionId,
                    Qty = qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }

            //update StockOnHold set ExpiryDate = {0} where SessionId = {1}
            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            //commit transaction
            return _ctx.SaveChangesAsync();
        }

        public async Task<bool> RemoveStockFromHold(string sessionId)
        {
            try
            {
                var stockOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == sessionId).ToList();

                _ctx.StocksOnHold.RemoveRange(stockOnHold);
                return await _ctx.SaveChangesAsync() > 0;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task RetrieveExpiredStockOnHold()
        {
            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stocksOnHold.Count > 0)
            {
                var stockToReturn = _ctx.Stocks.Where(x => stocksOnHold.Any(y => y.StockId == x.Id)).ToList();

                foreach (var stock in stockToReturn)
                {
                    stock.Qty += stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Qty;
                }

                _ctx.StocksOnHold.RemoveRange(stocksOnHold);

                return _ctx.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        public Task<int> UpdateStockRange(List<Stock> stocks)
        {
            _ctx.UpdateRange(stocks);
            return _ctx.SaveChangesAsync();
        }
        public StocksOnHold GetStocksOnHoldByIdWithSessionId(int stockId, string sessionId)
        {
            return _ctx.StocksOnHold
                .FirstOrDefault(x => x.StockId == stockId
                && x.SessionId == sessionId);
        }

        public Task<int> RemoveStockFromHold(StocksOnHold stockOnHold)
        {
            _ctx.StocksOnHold.Remove(stockOnHold);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateStock(Stock stock)
        {
            _ctx.Stocks.Update(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateStockOnHold(StocksOnHold stockOnHold)
        {
            _ctx.StocksOnHold.Update(stockOnHold);

            return _ctx.SaveChangesAsync();
        }
    }
}
