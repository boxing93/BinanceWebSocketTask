using System;
using BinanceWebSocketTask.Application.CryptoPrice.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BinanceWebSocketTask.API.Controllers;
public class StatisticsController : ApiControllerBase
{
    [HttpGet]
    [Route("{symbol}/24hAvgPrice")]
    [ResponseCache(VaryByQueryKeys = ["*"], Duration = 300)]
    public async Task<decimal> Get24AveragePrice([FromRoute] string symbol)
    {
        return await Mediator.Send(new Get24HoursAvgPriceQuery
        {
            Symbol = symbol
        });
    }

    [HttpGet]
    [Route("{symbol}/SimpleMovingAverage")]
    [ResponseCache(VaryByQueryKeys = ["*"], Duration = 300)]
    public async Task<decimal?> GetSimpleMovingAverage([FromRoute] string symbol, [FromQuery] GetSimpleMovingAveragePriceQuery query)
    {
        query.Symbol = symbol;
        return await Mediator.Send(query);
    }
}