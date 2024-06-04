using System;
namespace BinanceWebSocketTask.Domain.Entities
{
	public class CryptoSymbol : BaseEntity
	{
        public string Name { get; set; }
		public ICollection<CryptoPrice> Prices { get; set; }
	}
}

