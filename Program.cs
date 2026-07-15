using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

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

var json = File.ReadAllText("appsettings.json");

var settings = JsonSerializer.Deserialize<AppSettings>(json);

Console.WriteLine(settings.EmailSettings.EmailTo);
Console.WriteLine(settings.EmailSettings.SmtpHost);

public class ApiResponse
{
    public List<StockResult> Results { get; set; } = new();
}

public class StockResult
{
    public string Symbol { get; set; } = string.Empty;
    public decimal RegularMarketPrice { get; set; }
}

public class EmailSettings
{
    public string EmailTo { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public bool EnableSsl { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;


}

public class AppSettings
{
    public EmailSettings EmailSettings { get; set; } = new();
}