using BinanceWebSocketTask.Domain.Entities;

namespace BinanceWebSocketTask.Application;

public interface IRepository
{
    Task<List<T>> GetAll<T>() where T : BaseEntity;
    Task<T> AddAsync<T>(T entity) where T : BaseEntity;
    Task<int> UpdateAsync<T>(T entity) where T : BaseEntity;
    Task<int> UpdateAsync<T>(int id) where T : BaseEntity;
    Task<int> DeleteAsync<T>(T entity) where T : BaseEntity;
}
