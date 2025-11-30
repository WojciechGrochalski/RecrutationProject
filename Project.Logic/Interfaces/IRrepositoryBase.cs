namespace Project.Logic.Interfaces;

public interface IRrepositoryBase<TEntity>
{
    public Task<TEntity?> GetByIdAsync(Guid id);
    public Task<IEnumerable<TEntity>> GetByAllAsync();
    public Task<bool> AddAsync(TEntity entity);
    public Task<bool> UpdateAsync(TEntity entity);
    public Task<bool> RemoveAsync(TEntity entity);
}
