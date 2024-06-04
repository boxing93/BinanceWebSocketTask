using System;
using BinanceWebSocketTask.Application;
using BinanceWebSocketTask.Domain.Entities;
using BinanceWebSocketTask.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BinanceWebSocketTask.Infrastructure.Repositories;

public class BaseRepository : IRepository
{
    protected readonly DatabaseContext _dbContext;

    public BaseRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<T>> GetAll<T>() where T : BaseEntity
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();

    }

    public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<int> UpdateAsync<T>(T entity) where T : BaseEntity
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync<T>(T entity) where T : BaseEntity
    {
        _dbContext.Set<T>().Remove(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync<T>(int id) where T : BaseEntity
    {
        var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(u => u.Id == id);
        if (entity == null) return 0;
        _dbContext.Set<T>().Update(entity);
        return await _dbContext.SaveChangesAsync();
    }
}