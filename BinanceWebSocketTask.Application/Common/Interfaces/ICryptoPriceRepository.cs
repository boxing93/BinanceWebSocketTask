using System;
namespace BinanceWebSocketTask.Application.Common.Interfaces;
public interface ICryptoPriceRepository
{
    Task<decimal> GetPricesForLast24hoursAsync(string symbol);
    Task<decimal?> GetSimpleMovingAverageAsync(string symbol, DateTime? startDate);
}