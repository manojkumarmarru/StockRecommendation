using System;
using System.Collections.Generic;

public class DCFCalculator
{
    public List<decimal> CalculateDCF(List<StockData> stockDataList, List<BalanceSheetData> balanceSheetList, List<IncomeStatementData> incomeStatementList, List<WACCData> waccData, decimal growthRate)
    {
        var dcfValues = new List<decimal>();

        for (int i = 0; i < stockDataList.Count; i++)
        {
            var stockData = stockDataList[i];
            var balanceSheet = balanceSheetList[i];
            var incomeStatement = incomeStatementList[i];

            // Market Cap = Weighted Average Shares Outstanding Diluted * Stock Price
            var marketCap = incomeStatement.WeightedAverageShsOutDil * stockData.Price;

            // Enterprise Value NB = Market Cap + Long Term Debt + Short Term Debt
            decimal enterpriseValue = marketCap + balanceSheet.longTermDebt + balanceSheet.shortTermDebt;

            // Equity Value = Enterprise Value NB - Net Debt
            decimal equityValue = enterpriseValue - balanceSheet.netDebt;

            // DCF = Equity Value / Weighted Average Shares Outstanding Diluted
            decimal dcf = equityValue / incomeStatement.WeightedAverageShsOutDil;

            // Adjust DCF with growth rate (simplified example)
            dcf *= (1 + growthRate);

            dcfValues.Add(dcf);
        }

        return dcfValues;
    }
}