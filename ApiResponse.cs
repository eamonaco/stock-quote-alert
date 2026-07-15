using System;
using System.Collections.Generic;
using System.Text;

namespace Response;

public class ApiResponse
{
    public List<StockResult> Results { get; set; } = new();
}

public class StockResult
{
    public string Symbol { get; set; } = string.Empty;
    public decimal RegularMarketPrice { get; set; }
}
