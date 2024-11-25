using AutoMapper;
using AutoMapper.QueryableExtensions;
using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace NotebookLM.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly NotebookLMDbContext _dbContext;
    protected readonly IMapper _mapper;


    public GenericRepository(NotebookLMDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public GenericRepository(NotebookLMDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);

    }


    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }



    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;

    }




    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
