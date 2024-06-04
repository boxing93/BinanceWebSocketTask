using System;
namespace BinanceWebSocketTask.Application.Common.Models.Http.Responses;

public class CryptoPricesResponse
{
    public decimal BtcUsdtPrice { get; set; }
    public decimal AdaUsdtPrice { get; set; }
    public decimal EthUsdtPrice { get; set; }
}