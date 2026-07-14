using System.Net.Http;
using System.Net.Http.Json;

async Task<decimal> GetStockPriceAsync(string ticker)
{
    HttpClient client = new HttpClient();
    var url = $"https://brapi.dev/api/quote/{ticker}";
    try
    {
        var response = await client.GetFromJsonAsync<ApiResponse>(url);
        if (response != null)
        {
            return response.Results[0].RegularMarketPrice;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return 0;
}

var preco = await GetStockPriceAsync("PETR4");
Console.WriteLine(preco);

public class ApiResponse
{
    public List<StockResult> Results { get; set; } = new();
}

public class StockResult
{
    public string Symbol { get; set; } = string.Empty;
    public decimal RegularMarketPrice { get; set; }
}