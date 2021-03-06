﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Semantics;
using Microsoft.EntityFrameworkCore;
using PizzaWebshopCore2.Models;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredients)
                .HasForeignKey(di => di.IngredientId);


            builder.Entity<OrderedDishIngredient>()
                .HasKey(odi => new { odi.OrderedDishId, odi.IngredientId });

            builder.Entity<OrderedDishIngredient>()
                .HasOne(odi => odi.OrderedDish)
                .WithMany(od => od.OrderedDishesIngredients)
                .HasForeignKey(odi => odi.OrderedDishId);


            builder.Entity<OrderedDishIngredient>()
                .HasOne(odi => odi.Ingredient)
                .WithMany(i => i.OrderedDishIngredients)
                .HasForeignKey(odi => odi.IngredientId);

            

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<DishIngredient> DishIngredients { get; set; }

        public virtual DbSet<OrderedDish> OrderedDishes { get; set; }
        public virtual DbSet<OrderedDishIngredient> OrderedDishIngredients { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        
       
        
    }
}
