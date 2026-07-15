using Response;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Integration;

public class GetStockPrice
{
    public async Task<decimal> GetStockPriceAsync(string ticker)
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

}
