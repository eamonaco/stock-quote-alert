
using System.Text.Json;
using Settings;
using Integration;
using Email;

var json = File.ReadAllText("appsettings.json");
var settings = JsonSerializer.Deserialize<AppSettings>(json);

decimal? sendPrice = null;

GetStockPrice get = new();
EmailSender emailSender = new();

while (true)
{
    Console.WriteLine("Começo do while");
    var price = await get.GetStockPriceAsync(args[0]);
    var sellPrice = decimal.Parse(args[1]);
    var buyPrice = decimal.Parse(args[2]);
    Console.WriteLine($"Preço = {price}\nsendPrice = {sendPrice}");
    if (price != sendPrice)
    {
        Console.WriteLine("Analisando preço ao enviar o email.");
        if (price > sellPrice)
        {
            Console.WriteLine("Venda!");
            await emailSender.SendEmail(settings!.EmailSettings, "ALERTA: É recomendável a venda do seu ativo", $"O ativo {args[0]} está acima do preço de referência para venda. \nPreço Atual: {price}");
            sendPrice = price;
            Console.WriteLine("Email enviado!");
        }
        else if (price < buyPrice)
        {
            Console.WriteLine("Compra");
            await emailSender.SendEmail(settings!.EmailSettings, "ALERTA: É recomendável a compra de um ativo", $"O ativo {args[0]} está abaixo do preço de referência para compra. \nPreço Atual: {price}");
            sendPrice = price;
            Console.WriteLine("Email enviado!");
        }

    }

    Console.WriteLine("Tempo de espera");
    await Task.Delay(TimeSpan.FromSeconds(10));

}





