using System;
namespace BinanceWebSocketTask.Domain.Entities;

public class CryptoPrice : BaseEntity
{
    public int SymbolId { get; set; }
    public decimal Price { get; set; }
    public CryptoSymbol Symbol { get; set; }
}