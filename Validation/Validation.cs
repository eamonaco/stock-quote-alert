namespace Validation;
public class Validator : IValidation
{
    public void ValidateArgs(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("ERRO: Nenhum argumento foi informado.");
            return;
        }
        else if (args.Length != 3)
        {
            Console.WriteLine($"ERRO: Quantidade inválida de argumentos ({args.Length} informado(s)).");
            return;
        }

        if (!decimal.TryParse(args[1], out var sellPrice))
        {
            Console.WriteLine("ERRO: Preço de venda inválido.");
            return;

        }
        if (!decimal.TryParse(args[2], out var buyPrice))
        {
            Console.WriteLine("ERRO: Preço de compra inválido");
            return;
        }
    }
}
