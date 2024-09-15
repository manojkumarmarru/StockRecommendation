using System.Collections.Generic;
using System.Linq;

public class GrowthRateCalculator
{
    public decimal CalculateAverageGrowthRate(List<HistoricalRevenue> historicalRevenues)
    {
        if (historicalRevenues.Count < 2)
        {
            return 0;
        }

        var growthRates = new List<decimal>();

        for (int i = 1; i < historicalRevenues.Count; i++)
        {
            var previousRevenue = historicalRevenues[i - 1].Revenue;
            var currentRevenue = historicalRevenues[i].Revenue;

            if (previousRevenue != 0)
            {
                var growthRate = (currentRevenue - previousRevenue) / previousRevenue;
                growthRates.Add(growthRate);
            }
        }

        return growthRates.Average();
    }
}