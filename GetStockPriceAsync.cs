using Response;
using System.Net.Http.Json;

namespace Integration;

public class GetStockPrice
{
    public async Task<decimal?> GetStockPriceAsync(string ticker)
        {
            HttpClient client = new HttpClient();
            var url = $"https://brapi.dev/api/quote/{ticker}";
            try
            {
                var call = await client.GetAsync(url);

                call.EnsureSuccessStatusCode();

                var response = await call.Content.ReadFromJsonAsync<ApiResponse>();

                if (response != null)
                {
                    return response.Results[0].RegularMarketPrice;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return null;
        }

}
