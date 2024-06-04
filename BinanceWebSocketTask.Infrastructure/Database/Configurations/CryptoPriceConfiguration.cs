using System;
using BinanceWebSocketTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinanceWebSocketTask.Infrastructure.Database.Configurations;

public class CryptoPriceConfiguration : IEntityTypeConfiguration<CryptoPrice>
{
    public void Configure(EntityTypeBuilder<CryptoPrice> builder)
    {
        builder.ToTable("CryptoPrices");

        builder.Property(e => e.Id)
                   .UseIdentityColumn();

        builder.HasOne(cp => cp.Symbol)
            .WithMany(cp => cp.Prices);
    }
}