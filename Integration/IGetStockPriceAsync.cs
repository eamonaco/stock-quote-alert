namespace IntegrationInterface;

public interface IGetStockPrice
{
    Task<decimal?> GetStockPriceAsync(string ticker);
}