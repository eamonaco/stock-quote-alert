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

decimal? sendPrice = null;

while (true)
{
    Console.WriteLine("Começo do while");
    var price = await GetStockPriceAsync(args[0]);
    var sellPrice = decimal.Parse(args[1]);
    var buyPrice = decimal.Parse(args[2]);
    Console.WriteLine($"Preço = {price}\nsendPrice = {sendPrice}");
    if (price != sendPrice)
    {
        Console.WriteLine("Analisando preço ao enviar o email.");
        if (price > sellPrice)
        {
            Console.WriteLine("Venda!");
            await EnviarEmail(settings.EmailSettings, "ALERTA: É recomendável a venda do seu ativo", $"O ativo {args[0]} está acima do preço de referência para venda. \nPreço Atual: {price}");
            sendPrice = price;
            Console.WriteLine("Email enviado!");
        }
        else if (price < buyPrice)
        {
            Console.WriteLine("Compra");
            await EnviarEmail(settings.EmailSettings, "ALERTA: É recomendável a compra de um ativo", $"O ativo {args[0]} está abaixo do preço de referência para compra. \nPreço Atual: {price}");
            sendPrice = price;
            Console.WriteLine("Email enviado!");
        }

    }

    Console.WriteLine("Tempo de espera");
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

