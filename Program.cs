using System.Net;
using System.Net.Mail;
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

async Task EnviarEmail(EmailSettings config, string assunto, string corpo)
{
    using var mensagem = new MailMessage();

    mensagem.From = new MailAddress(config.From);
    mensagem.To.Add(config.EmailTo);
    mensagem.Subject = assunto;
    mensagem.Body = corpo;
    mensagem.IsBodyHtml = false;

    using var smtp = new SmtpClient(config.SmtpHost, config.SmtpPort);

    smtp.Credentials = new NetworkCredential(
        config.Username,
        config.Password);

    smtp.EnableSsl = config.EnableSsl;

    await smtp.SendMailAsync(mensagem);
}


var json = File.ReadAllText("appsettings.json");

var settings = JsonSerializer.Deserialize<AppSettings>(json);

while (true)
{
    var price = await GetStockPriceAsync(args[0]);
    var sellPrice = decimal.Parse(args[1]);
    var buyPrice = decimal.Parse(args[2]);
    if (price > sellPrice)
    {
        await EnviarEmail(settings.EmailSettings, "ALERTA: É recomendável a venda do seu ativo", $"Você possui um ativo na bolsa {args[0]} que está acima do preço que foi comprado. Recomendamos a venda nesse exato momento. \nAgora: {price}");
    }
    else if (price < buyPrice)
    {
        await EnviarEmail(settings.EmailSettings, "ALERTA: É recomendável a compra de um ativo", $"O ativo {args[0]} está com um preço abaixo do que normal. Recomendamos a compra nesse exato momento. \nAgora: {price}");
    }
    else
    {
        await EnviarEmail(settings.EmailSettings, "ALERTA: É recomendável não realizar operações com seu ativo", $"Você possui um ativo na bolsa {args[0]} que está dentro da faixa média de preço. Recomendamos não realizar nenhuma operação no momento. \nAgora: {price}");
    }

    await Task.Delay(TimeSpan.FromSeconds(60));
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

