using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FinancialDataService
{
    private readonly HttpClient client;
    private const string apiKey = "T3REvePztwVSl7HPnXrR9bBOErhAKROJ";  // Add your API key here

    public FinancialDataService(HttpClient httpClient)
    {
        client = httpClient;
    }

    public async Task<List<StockData>> GetQuote(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/quote/{symbol}?apikey={apiKey}";
        var response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        var stockDataArray = JsonConvert.DeserializeObject<List<StockData>>(responseBody);
        return stockDataArray;  // Return the entire list
    }

    public async Task<List<BalanceSheetData>> FetchBalanceSheetData(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/balance-sheet-statement/{symbol}?apikey={apiKey}";
        var response = await client.GetStringAsync(url);
        var balanceSheets = JsonConvert.DeserializeObject<List<BalanceSheetData>>(response);
        return balanceSheets;  // Return the entire list
    }

    public async Task<List<IncomeStatementData>> FetchIncomeStatementData(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/income-statement/{symbol}?apikey={apiKey}";
        var response = await client.GetStringAsync(url);
        var incomeStatements = JsonConvert.DeserializeObject<List<IncomeStatementData>>(response);
        return incomeStatements;  // Return the entire list
    }

    public async Task<List<HistoricalRevenue>> FetchHistoricalRevenue(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/income-statement/{symbol}?apikey={apiKey}";
        var response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        var incomeStatements = JsonConvert.DeserializeObject<List<IncomeStatementData>>(responseBody);

        var historicalRevenue = new List<HistoricalRevenue>();
        foreach (var statement in incomeStatements)
        {
            historicalRevenue.Add(new HistoricalRevenue
            {
                Year = statement.date.Year,
                Revenue = statement.Revenue
            });
        }
        return historicalRevenue;
    }

     public async Task<List<WACCData>> FetchWACCData(string symbol)
    {
        // Fetch market capitalization
        var stockDataList = await GetQuote(symbol);
        var balanceSheetList = await FetchBalanceSheetData(symbol);
        var incomeStatementList = await FetchIncomeStatementData(symbol);

        var waccDataList = new List<WACCData>();

        for (int i = 0; i < stockDataList.Count; i++)
        {
            decimal marketValueOfEquity = stockDataList[i].MarketCap;  // Use the corresponding element for market cap
            decimal marketValueOfDebt = balanceSheetList[i].totalDebt;  // Use the corresponding element for total debt
            decimal corporateTaxRate = CalculateTaxRate(incomeStatementList[i]);  // Use the corresponding element for tax rate calculation

            // Fetch key metrics data
            string url = $"https://financialmodelingprep.com/api/v3/company-key-metrics/{symbol}?apikey={apiKey}";
            var response = await client.GetStringAsync(url);
            var keyMetrics = JsonConvert.DeserializeObject<KeyMetricsData>(response);

            // Calculate Cost of Equity using CAPM
            decimal costOfEquity = keyMetrics.RiskFreeRate + keyMetrics.Beta * (keyMetrics.MarketReturn - keyMetrics.RiskFreeRate);

            // Calculate Cost of Debt
            decimal costOfDebt = keyMetrics.InterestCoverage != 0 ? 1 / keyMetrics.InterestCoverage : 0;

            waccDataList.Add(new WACCData
            {
                MarketValueOfEquity = marketValueOfEquity,
                MarketValueOfDebt = marketValueOfDebt,
                CostOfEquity = costOfEquity,
                CostOfDebt = costOfDebt,
                CorporateTaxRate = corporateTaxRate
            });
        }

        return waccDataList;
    }

    private decimal CalculateTaxRate(IncomeStatementData incomeStatement)
    {
        if (incomeStatement.IncomeBeforeTax == 0)
        {
            return 0;
        }
        return incomeStatement.IncomeTaxExpense / incomeStatement.IncomeBeforeTax;
    }
}