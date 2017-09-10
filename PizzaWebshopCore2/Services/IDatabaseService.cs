using System.Linq;
using PizzaWebshopCore2.Data;

namespace PizzaWebshopCore2.Services
{
    public interface IDatabaseService
    {
        void SaveChanges();
        void Add<TEntity>(TEntity entity) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;

    }

    public class DatabaseService : IDatabaseService
    {
        private readonly IApplicationDbContext _context;
        public DatabaseService(IApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Add(entity);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet;
        }
        
    } 
    
}
