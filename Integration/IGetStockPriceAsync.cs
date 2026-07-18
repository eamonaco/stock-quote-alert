namespace Integration;

public interface IGetStockPrice
{
    Task<decimal?> GetStockPriceAsync(string ticker);
}