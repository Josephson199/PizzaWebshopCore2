using System.Linq;
using System.Threading.Tasks;
using PizzaWebshopCore2.Data;

namespace PizzaWebshopCore2.Services
{
    public interface IDatabaseService
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Add<TEntity>(TEntity entity) where TEntity : class;
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(object key) where TEntity : class;
        Task<TEntity> GetAsync<TEntity>(object key) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;

    }

    public class DatabaseService : IDatabaseService
    {
        private readonly IApplicationDbContext _context;
        public DatabaseService(IApplicationDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
           return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
           return _context.SaveChangesAsync();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Add(entity);
        }

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.AddAsync(entity);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet;
        }

        public TEntity Get<TEntity>(object key) where TEntity : class
        {
            return _context.Find<TEntity>(key);
        }

        public Task<TEntity> GetAsync<TEntity>(object key) where TEntity : class
        {
            return _context.FindAsync<TEntity>(key);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Remove(entity);
        }
    } 
    
}
