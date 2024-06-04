using System;
namespace BinanceWebSocketTask.Domain.Entities;
public class BaseEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
}