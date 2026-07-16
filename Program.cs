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

    if (args.Length == 3)
    {
        if (!decimal.TryParse(args[1], out var sellPrice))
        {
            Console.WriteLine("ERRO: Preço de venda inválido.");
            Environment.Exit(0);

        }
        if (!decimal.TryParse(args[2], out var buyPrice))
        {
            Console.WriteLine("ERRO: Preço de compra inválido");
            Environment.Exit(0);
        }

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

        Console.WriteLine("Tempo de espera");
        await Task.Delay(TimeSpan.FromSeconds(10));
    }
    else if (args.Length == 0)
    {
        Console.WriteLine("ERRO: Nenhum argumento foi informado.");
        Environment.Exit(0);
    }
    else
    {
        Console.WriteLine($"ERRO: Quantidade inválida de argumentos ({args.Length} informado(s)).");
        Environment.Exit(0);
    }

}





