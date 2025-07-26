namespace Lab5.Persistence.Data.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    //Task SaveChangesAsync();
}