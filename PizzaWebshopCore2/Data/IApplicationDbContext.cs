using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Data
{
    public interface IApplicationDbContext: IDisposable
    {
        DbSet<Dish> Dishes { get; set; }
        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<DishIngredient> DishIngredients { get; set; }
        DbSet<OrderedDish> OrderedDishes { get; set; }
        DbSet<OrderedDishIngredient> OrderedDishIngredients { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;
       
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;
    }
}
