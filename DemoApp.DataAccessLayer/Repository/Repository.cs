using System.Linq.Expressions;
using DemoApp.DataAccessLayer.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.DataAccessLayer.Repository;

public interface IRepository<T> where T : class
{
    void Add(T objModel);
    void AddRange(IEnumerable<T> objModels);
    T GetId(int id);
    Task<T> GetIdAsync(int id);
    T Get(Expression<Func<T, bool>> predicate);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
    int Count();
    Task<int> CountAsync();
    void Update(T objModel);
    void Remove(T objModel);
    int SaveChanges();
    Task<int> SaveChangesAsync();
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly PostgresDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(PostgresDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T objModel) => _dbSet.Add(objModel);

    public void AddRange(IEnumerable<T> objModels) => _dbSet.AddRange(objModels);

    public T GetId(int id) => _dbSet.Find(id);

    public async Task<T> GetIdAsync(int id) => await _dbSet.FindAsync(id);

    public T Get(Expression<Func<T, bool>> predicate) => _dbSet.FirstOrDefault(predicate);

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

    public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).ToList();

    public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public int Count() => _dbSet.Count();

    public async Task<int> CountAsync() => await _dbSet.CountAsync();

    public void Update(T objModel) => _dbSet.Update(objModel);

    public void Remove(T objModel) => _dbSet.Remove(objModel);

    public int SaveChanges() => _context.SaveChanges();

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}