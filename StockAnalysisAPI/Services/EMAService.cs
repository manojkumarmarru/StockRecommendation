using System;
using System.Collections.Generic;
using System.Linq;

public class EMAService
{
    public List<decimal> CalculateEMA(List<decimal> prices, int period)
    {
        if (prices == null || prices.Count < period)
        {
            throw new ArgumentException("Insufficient data to calculate EMA.");
        }

        List<decimal> emaValues = new List<decimal>();
        decimal multiplier = 2m / (period + 1);
        decimal ema = prices.Take(period).Average(); // Initial EMA value

        emaValues.Add(ema);

        for (int i = period; i < prices.Count; i++)
        {
            ema = ((prices[i] - ema) * multiplier) + ema;
            emaValues.Add(ema);
        }

        return emaValues;
    }
}