using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace BinanceWebSocketTask.Application.Common.Models.Binance
{
	public class BinanceSubscribeMessage
	{
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public string[] Params { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
	}
}

