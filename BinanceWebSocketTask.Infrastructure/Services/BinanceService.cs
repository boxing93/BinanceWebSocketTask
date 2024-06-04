using System;
using BinanceWebSocketTask.Application.Common.Interfaces;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using BinanceWebSocketTask.Application;
using BinanceWebSocketTask.Domain.Entities;
using BinanceWebSocketTask.Application.Common.Models.Binance;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using BinanceWebSocketTask.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using BinanceWebSocketTask.Application.Common.Exceptions;

namespace BinanceWebSocketTask.Infrastructure.Services;

public class BinanceService : IBinanceService
{
    private readonly WebSocket _webSocket;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BinanceService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _webSocket = new WebSocket("wss://stream.binance.com:9443/ws");
        _webSocket.OnMessage += WebSocket_OnMessage;
        _webSocket.OnOpen += WebSocket_OnOpen;
        _webSocket.OnError += WebSocket_OnError;
        _webSocket.Connect();
    }

    private void WebSocket_OnError(object? sender, WebSocketSharp.ErrorEventArgs e)
    {
    }

    private void WebSocket_OnOpen(object? sender, EventArgs e)
    {
        var symbols = new List<string> { "btcusdt", "adausdt", "ethusdt" };
        var streams = string.Join(",", symbols.ConvertAll(symbol => $"{symbol}@ticker")).Split(",");
        var subscribeMessage = new BinanceSubscribeMessage
        {
            Method = "SUBSCRIBE",
            Params = streams,
            Id = 1
        };
        var json = JsonConvert.SerializeObject(subscribeMessage).ToString();
        _webSocket.Send(json);
    }

    private async void WebSocket_OnMessage(object? sender, MessageEventArgs e)
    {
        var data = JObject.Parse(e.Data);
        var price = data["c"]?.ToString();
        var symbol = data["s"]?.ToString();

        if (!string.IsNullOrEmpty(symbol) && !string.IsNullOrEmpty(price))
        {
           await InsertDataToDatabase(price, symbol);
        }
    }

    private async Task InsertDataToDatabase(string price, string symbol)
    {
        var successfullyParsed = decimal.TryParse(price, out decimal parsedPrice);
        if (!successfullyParsed)
            throw new Exception($"Invalid price: {price}");

        var dbContext = InitializeDatabaseContext();
        var cryptoSymbols = dbContext.CryptoSymbols
                                            .AsNoTracking()
                                            .ToList();

        var symbolId = cryptoSymbols?.FirstOrDefault(cs => cs.Name == symbol)?.Id;

        if (symbolId == null) throw new NotFoundException($"Symbol name is invalid or not found.");
        
        await dbContext.AddAsync(new CryptoPrice
        {
            SymbolId = (int)symbolId,
            Price = parsedPrice,
            Created = DateTime.Now
        });
        await dbContext.SaveChangesAsync();
    }

    private DatabaseContext InitializeDatabaseContext()
    {
        var scope = _serviceScopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    }
}