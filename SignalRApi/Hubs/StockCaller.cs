using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SignalRApi.Entities;

namespace SignalRApi.Hubs
{
    public class StockCaller
    {
        public static ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        //public static Dictionary<string, Stock> _stock = new Dictionary<string, Stock>();
        public StockCaller()
        {
            AddValue();
        }
        public async Task AddValue()
        {
            Random random = new Random();

            string[] symbols = new string[6] { "USD", "EUR", "ATLAS", "GARAN", "ISBNK", "AKBNK" };
            foreach (var item in symbols)
            {
                Stock stock = new Stock()
                {
                    symbol = item,
                    price = random.Next(100, 500),
                    percent = random.NextDouble(),
                };
                //if (_stock.ContainsKey(stock.symbol))
                //    _stock[stock.symbol] = stock;
                //else
                //_stock.Add(stock.symbol, stock);
                _stocks.TryAdd(stock.symbol, stock);
            }

            await Task.CompletedTask;
            //_stock.Add(random.Next(1000, 9999).ToString());
        }
        public async Task<ConcurrentDictionary<string,Stock>> GetValues()
        {
            return await Task.FromResult(_stocks);
        }

        public void ClearStock()
        {
            _stocks.Clear();
        }

    }
}

