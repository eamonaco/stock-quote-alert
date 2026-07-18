namespace Validation;
public class Validator : IValidation
{
    public void ValidateArgs(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("ERRO: Nenhum argumento foi informado.");
            Environment.Exit(0);
        }
        else if (args.Length != 3)
        {
            Console.WriteLine($"ERRO: Quantidade inválida de argumentos ({args.Length} informado(s)).");
            Environment.Exit(0);
        }

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
    }
}
