using System;
using BinanceWebSocketTask.Application.Common.Enums;
using BinanceWebSocketTask.Application.Common.Interfaces;
using BinanceWebSocketTask.Application.Common.Models.Http.Responses;
using BinanceWebSocketTask.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BinanceWebSocketTask.Infrastructure.Repositories;

public class CryptoPriceRepository : BaseRepository, ICryptoPriceRepository
{

    public CryptoPriceRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public async Task<decimal> GetPricesForLast24hoursAsync(string symbol)
    {
        var last24Hours = DateTime.Now.AddHours(-24);

        var last24HoursAveragePrice = await _dbContext.CryptoPrices
                                                        .Include(cp => cp.Symbol)
                                                        .AsNoTracking()
                                                        .Where(cp => cp.Created >= last24Hours && cp.Symbol.Name == symbol)
                                                        .AverageAsync(cp => cp.Price);

        if (last24HoursAveragePrice > 0)
        {
            return last24HoursAveragePrice;

        }
        return await _dbContext.CryptoPrices
                    .AsNoTracking()
                    .OrderBy(cp => cp.Created)
                    .AverageAsync(cp => cp.Price);
    }

    public async Task<decimal?> GetSimpleMovingAverageAsync(string symbol, DateTime? startDate)
    {
        var prices = _dbContext.CryptoPrices
                     .Include(cp => cp.Symbol)
                     .Where(cp => cp.Symbol.Name == symbol && cp.Created >= startDate)
                     .Select(cp => cp.Price);

        if (prices != null && prices.Any()) return await prices.AverageAsync();

        return null;
    }
}