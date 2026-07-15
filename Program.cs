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

while (true)
{
    var price = await GetStockPriceAsync(args[0]);
    var sellPrice = decimal.Parse(args[1]);
    var buyPrice = decimal.Parse(args[2]);
    Console.WriteLine($"agora: {price}");
    if (price > sellPrice)
    {
        Console.WriteLine("ALERTA: É recomendável a VENDA.");
    }
    else if (price < buyPrice)
    {
        Console.WriteLine("ALERTA: É recomendável a COMPRA.");
    }
    else
    {
        Console.WriteLine("ALERTA: Nenhuma ação recomendada.");
    }
    ;
    await Task.Delay(TimeSpan.FromSeconds(10));
}

public class ApiResponse
{
    public List<StockResult> Results { get; set; } = new();
}

public class StockResult
{
    public string Symbol { get; set; } = string.Empty;
    public decimal RegularMarketPrice { get; set; }
}