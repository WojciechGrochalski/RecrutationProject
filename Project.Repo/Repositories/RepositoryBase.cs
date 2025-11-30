using Microsoft.EntityFrameworkCore;
using Project.Logic.Interfaces;
using Project.Logic.Models;

namespace Project.Repo.Repositories;

public class RepositoryBase<TEntity> : IRrepositoryBase<TEntity> where TEntity : BaseModel
{
    protected readonly ProjectDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(ProjectDbContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<TEntity>();
    }


    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<TEntity>> GetByAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
