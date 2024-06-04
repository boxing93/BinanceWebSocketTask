using System;
using BinanceWebSocketTask.Application;
using BinanceWebSocketTask.Application.Common.Enums;
using BinanceWebSocketTask.Application.Common.Exceptions;
using BinanceWebSocketTask.Application.Common.Interfaces;
using BinanceWebSocketTask.Domain.Entities;

namespace BinanceWebSocketTask.Infrastructure.Services;
public class StatisticService : IStatisticService
{
    private readonly ICryptoPriceRepository _cryptoPriceRepository;
    private readonly IRepository _repository;

    public StatisticService(ICryptoPriceRepository cryptoPriceRepository, IRepository repository)
    {
        _cryptoPriceRepository = cryptoPriceRepository;
        _repository = repository;
    }

    public async Task<decimal> GetAveragePricesForLast24hoursAsync(string symbol)
    {
        await SymbolExistCheckAsync(symbol);

        return await _cryptoPriceRepository.GetPricesForLast24hoursAsync(symbol);
    }
    public async Task<decimal?> GetSimpleMovingAverage(string symbol, int dataPointsAmount, string timePeriod, DateTime? startDate)
    {
        await SymbolExistCheckAsync(symbol);

        var timePeriodInMinutes = GetTimePeriodInMinutes(timePeriod);
        var calculatedTimePeriod = dataPointsAmount * timePeriodInMinutes;

        if (startDate == null)
            startDate = DateTime.Now.AddMinutes(-calculatedTimePeriod);
        else
            startDate.Value.AddMinutes(-calculatedTimePeriod); 
        
        return await _cryptoPriceRepository.GetSimpleMovingAverageAsync(symbol,startDate);
    }

    private async Task SymbolExistCheckAsync(string symbol)
    {
        var symbols = await _repository.GetAll<CryptoSymbol>();
        if (!symbols.Select(s => s.Name.ToLower()).Contains(symbol.ToLower())) throw new NotFoundException($"{symbol} not found");
    }

    private static long GetTimePeriodInMinutes(string timePeriod)
    {
        switch (timePeriod)
        {
            case "1w":
                return 10080;
            case "1d":
                return 1440;
            case "30m":
                return 30;
            case "5m":
                return 5;
            case "1m":
                return 1;
            default:
                return 0;
        }
    }
}