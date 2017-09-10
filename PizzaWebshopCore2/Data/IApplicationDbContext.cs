using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
