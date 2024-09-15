using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StockAnalysisService
{
    private readonly FinancialDataService _financialDataService;
    private readonly GrowthRateCalculator _growthRateCalculator;
    private readonly DCFCalculator _dcfCalculator;

    public StockAnalysisService(FinancialDataService financialDataService, GrowthRateCalculator growthRateCalculator, DCFCalculator dcfCalculator)
    {
        _financialDataService = financialDataService;
        _growthRateCalculator = growthRateCalculator;
        _dcfCalculator = dcfCalculator;
    }

    public async Task<StockAnalysisResult> AnalyzeStock(string symbol)
    {
        var stockDataList = await _financialDataService.GetQuote(symbol);
        var balanceSheetList = await _financialDataService.FetchBalanceSheetData(symbol);
        var incomeStatementList = await _financialDataService.FetchIncomeStatementData(symbol);
        var waccData = await _financialDataService.FetchWACCData(symbol);
        var historicalRevenue = await _financialDataService.FetchHistoricalRevenue(symbol);

        if (!stockDataList.Any() || !balanceSheetList.Any() || !incomeStatementList.Any())
        {
            throw new Exception("Insufficient data to perform stock analysis.");
        }

        decimal growthRate = _growthRateCalculator.CalculateAverageGrowthRate(historicalRevenue);
        List<decimal> dcfValues = _dcfCalculator.CalculateDCF(stockDataList, balanceSheetList, incomeStatementList, waccData, growthRate);

        var stockAnalysisResults = new StockAnalysisResult
            {
                StockData = stockDataList,
                BalanceSheet = balanceSheetList,
                IncomeStatement = incomeStatementList,
                WACCData = waccData,
                GrowthRate = growthRate,
                HistoricalRevenue = historicalRevenue,
                DCFValues = dcfValues // Assuming dcfValues is a list of lists or similar structure
            };
        return stockAnalysisResults;
    }
}