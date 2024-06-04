using BinanceWebSocketTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BinanceWebSocketTask.Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DbSet<CryptoPrice> CryptoPrices { get; set; }
    public DbSet<CryptoSymbol> CryptoSymbols { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(builder);
    }
}