using System.Collections.Generic;

public class StockAnalysisResult
{
    public List<StockData> StockData { get; set; }
    public List<BalanceSheetData> BalanceSheet { get; set; }
    public List<IncomeStatementData> IncomeStatement { get; set; }
    public List<WACCData> WACCData { get; set; }
    public decimal GrowthRate { get; set; }
    public List<HistoricalRevenue> HistoricalRevenue { get; set; }
    public List<decimal> DCFValues { get; set; }
}