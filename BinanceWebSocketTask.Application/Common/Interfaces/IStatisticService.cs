using System;
namespace BinanceWebSocketTask.Application.Common.Interfaces;
public interface IStatisticService
{
    Task<decimal> GetAveragePricesForLast24hoursAsync(string symbol);
    Task<decimal?> GetSimpleMovingAverage(string symbol, int dataPointsAmount, string timePeriod, DateTime? startDate);
}