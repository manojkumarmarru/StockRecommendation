using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class StockAnalysisController : ControllerBase
{
    private readonly StockAnalysisService _stockAnalysisService;
    private readonly EMAService _emaService;

    public StockAnalysisController(StockAnalysisService stockAnalysisService, EMAService emaService)
    {
        _stockAnalysisService = stockAnalysisService;
        _emaService = emaService;
    }

    [HttpGet("analyze/{symbol}")]
    public async Task<IActionResult> AnalyzeStock(string symbol)
    {
        var result = await _stockAnalysisService.AnalyzeStock(symbol);
        return Ok(result);
    }

    [HttpPost("ema")]
    public IActionResult CalculateEMA([FromBody] List<decimal> prices, [FromQuery] int period)
    {
        try
        {
            var emaValues = _emaService.CalculateEMA(prices, period);
            return Ok(emaValues);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}