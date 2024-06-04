using System;
using BinanceWebSocketTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinanceWebSocketTask.Infrastructure.Database.Configurations
{
	public class CryptoSymbolConfiguration : IEntityTypeConfiguration<CryptoSymbol>
    {
        public void Configure(EntityTypeBuilder<CryptoSymbol> builder)
        {
            builder.ToTable("CryptoSymbol");

            builder.Property(e => e.Id)
                       .UseIdentityColumn();

            builder.HasMany(cs => cs.Prices)
                .WithOne(cs => cs.Symbol);
        }
    }
}

