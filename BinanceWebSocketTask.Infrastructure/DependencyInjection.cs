using System;
using BinanceWebSocketTask.Application;
using BinanceWebSocketTask.Application.Common.Interfaces;
using BinanceWebSocketTask.Infrastructure.Database;
using BinanceWebSocketTask.Infrastructure.Repositories;
using BinanceWebSocketTask.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BinanceWebSocketTask.Infrastructure
{
	public static class DependencyInjection
	{
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services
                    .AddScoped<IRepository, BaseRepository>()
                    .AddScoped<IStatisticService, StatisticService>()
                    .AddScoped<ICryptoPriceRepository, CryptoPriceRepository>()
                    .AddSingleton<IBinanceService, BinanceService>();

            return services;
        }
    }
}

