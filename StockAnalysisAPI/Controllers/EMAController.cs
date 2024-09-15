using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class EMAController : ControllerBase
{
    private readonly EMAService _emaService;
    private readonly HttpClient _httpClient;
    private string ApiKey = "T3REvePztwVSl7HPnXrR9bBOErhAKROJ";  // Add your API key here

    public EMAController(EMAService emaService, HttpClient httpClient)
    {
        _emaService = emaService;
        _httpClient = httpClient;
    }

    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetEMAData(string symbol, [FromQuery] int period)
    {
        string url = $"https://financialmodelingprep.com/api/v3/technical_indicator/5min/{symbol}?type=ema&period={period}&apikey={ApiKey}";
        var response = await _httpClient.GetStringAsync(url);
        var emaData = JsonConvert.DeserializeObject<List<EmaData>>(response);
        return Ok(emaData);
    }

    [HttpPost]
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