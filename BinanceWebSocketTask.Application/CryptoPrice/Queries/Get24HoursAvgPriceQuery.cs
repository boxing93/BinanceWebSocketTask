using System;
using BinanceWebSocketTask.Application.Common.Interfaces;
using MediatR;

namespace BinanceWebSocketTask.Application.CryptoPrice.Queries;

public class Get24HoursAvgPriceQuery : IRequest<decimal>
{
    /// <summary>
    ///  The symbol the average price is being calculated for.
    /// </summary>
    public string Symbol { get; set; }
}
internal class Get24HoursAvgPriceQueryHandler : IRequestHandler<Get24HoursAvgPriceQuery, decimal>
{
    private readonly IStatisticService _statisticService;

    public Get24HoursAvgPriceQueryHandler(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    public async Task<decimal> Handle(Get24HoursAvgPriceQuery request, CancellationToken cancellationToken)
    {
        return await _statisticService.GetAveragePricesForLast24hoursAsync(request.Symbol);
    }
}