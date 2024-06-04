using System;
using BinanceWebSocketTask.Application.Common.Interfaces;
using MediatR;

namespace BinanceWebSocketTask.Application.CryptoPrice.Queries;

public class GetSimpleMovingAveragePriceQuery : IRequest<decimal?>
{
    /// <summary>
    ///  The amount of data points.
    /// </summary>
    public int N { get; set; }

    /// <summary>
    ///  The time period represented by each data point.
    /// </summary>
    public string P { get; set; }

    /// <summary>
    ///  The symbol the average price is being calculated for.
    /// </summary>
    public string? Symbol { get; set; }

    /// <summary>
    ///  The datetime from which to start the SMA calculation.
    /// </summary>
    public DateTime? S { get; set; }
}

internal class GetSimpleMovingAveragePriceQueryHandler : IRequestHandler<GetSimpleMovingAveragePriceQuery, decimal?>
{
    private readonly IStatisticService _statisticService;

    public GetSimpleMovingAveragePriceQueryHandler(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    public async Task<decimal?> Handle(GetSimpleMovingAveragePriceQuery request, CancellationToken cancellationToken)
    {
        return await _statisticService.GetSimpleMovingAverage(request.Symbol, request.N, request.P, request.S);
    }
}
