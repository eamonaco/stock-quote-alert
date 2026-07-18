using System.Text.Json;
using Email;
using Integration;
using Validation;
using Settings;

var json = File.ReadAllText("appsettings.json");
var settings = JsonSerializer.Deserialize<AppSettings>(json);

decimal? sendPrice = null;

GetStockPrice get = new();
EmailSender emailSender = new();
Validator validator = new();

while (true)
{

    validator.ValidateArgs(args);

    var sellPrice = decimal.Parse(args[1]);
    var buyPrice = decimal.Parse(args[2]);

    Console.WriteLine("Consultando preço...");
    var price = await get.GetStockPriceAsync(args[0]);

    if (price == null)
    {
        Console.WriteLine("ERRO: Falha na requisição à API de preços.");
        Environment.Exit(0);

    }

    Console.WriteLine($"Preço = {price}\nsendPrice = {sendPrice}");
    if (price != sendPrice)
    {
        Console.WriteLine("\nAnalisando preço ao enviar o email...");
        if (price > sellPrice)
        {

            await emailSender.SendEmail(settings!.EmailSettings, "ALERTA: É recomendável a venda do seu ativo", $"O ativo {args[0]} está acima do preço de referência para venda. \nPreço Atual: {price}");
            sendPrice = price;

            Console.WriteLine("Email de venda enviado!");
        }
        else if (price < buyPrice)
        {

            await emailSender.SendEmail(settings!.EmailSettings, "ALERTA: É recomendável a compra de um ativo", $"O ativo {args[0]} está abaixo do preço de referência para compra. \nPreço Atual: {price}");
            sendPrice = price;

            Console.WriteLine("Email de compra enviado!");
        }

    }

    Console.WriteLine("Tempo de espera para a próxima requisição...");
    await Task.Delay(TimeSpan.FromSeconds(10));
    

}





