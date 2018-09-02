using System;
using System.Linq;

namespace CrossExchange
{
    public class TradeRepository : GenericRepository<Trade>, ITradeRepository
    {
        public TradeRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool BuyShare(TradeModel trade)
        {
            try
            {
                var share = _dbContext.Shares.Find(trade.ShareId);
                var portfolioUser = _dbContext.Portfolios.Find(trade.PortfolioId);

                if (share != null && share.Symbol.Length == 3 && share.Symbol.All(c => char.IsUpper(c)) && portfolioUser != null && !string.IsNullOrEmpty(trade.Action) && trade.Action.Equals("BUY"))
                {
                    var tde = new Trade
                    {
                        Action = trade.Action,
                        NoOfShares = trade.NoOfShares,
                        Price = (trade.Price*trade.NoOfShares),
                        PortfolioId = trade.PortfolioId,
                        Symbol = trade.Symbol
                    };

                    decimal latestPrice = trade.Price;
                    share.Rate = latestPrice;

                    _dbContext.Add(tde);
                    _dbContext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool SellShare(TradeModel trade)
        {
            try
            {
                var share = _dbContext.Shares.Find(trade.ShareId);
                var portfolioUser = _dbContext.Portfolios.Find(trade.PortfolioId);

                if (share != null && share.Symbol.Length == 3 && share.Symbol.All(c => char.IsUpper(c)) && portfolioUser != null && !string.IsNullOrEmpty(trade.Action) && trade.Action.Equals("SELL"))
                {
                    var boughtSharesList = _dbContext.Trades.Where(a => a.Price / a.NoOfShares == share.Rate && a.Symbol == trade.Symbol && a.Action == "BUY").ToList();
                    int boughtShares = 0;
                    foreach (var i in boughtSharesList)
                    {
                        boughtShares = boughtShares + i.NoOfShares;
                    }

                    var soldsharesList = _dbContext.Trades.Where(a => a.Price / a.NoOfShares == share.Rate && a.Symbol == trade.Symbol && a.Action == "SELL").ToList();
                    int soldShares = 0;
                    foreach (var j in soldsharesList)
                    {
                        soldShares = soldShares + j.NoOfShares;
                    }
                    var availableSharesTobeSold = (boughtShares - soldShares);

                    if (availableSharesTobeSold >= trade.NoOfShares)
                    {
                        var tde = new Trade
                        {
                            Action = trade.Action,
                            NoOfShares = trade.NoOfShares,
                            Price = (trade.Price * trade.NoOfShares),
                            PortfolioId = trade.PortfolioId,
                            Symbol = trade.Symbol
                        };

                        decimal latestPrice = trade.Price;
                        share.Rate = latestPrice;

                        _dbContext.Add(tde);
                        _dbContext.SaveChanges();

                        return true;
                    }          
                }

                   return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}