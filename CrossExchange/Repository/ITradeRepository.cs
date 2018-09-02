namespace CrossExchange
{
    public interface ITradeRepository : IGenericRepository<Trade>
    {
        bool BuyShare(TradeModel trade);
        bool SellShare(TradeModel trade);
    }
}