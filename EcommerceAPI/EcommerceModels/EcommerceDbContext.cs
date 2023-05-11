using EcommerceModels.Auth;
using EcommerceModels.MasterDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceModels
{
    public class EcommerceDbContext:DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        // Auth
        public DbSet<User> tblUser => Set<User>();
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set up two users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id= 1,
                    FirstName = "Md. Mahbur Rahman",
                    LastName = "Radif",
                    UserName = "mahbur",
                    Password = "R@dif2022",
                    Role = "admin",
                    Email = "mahbur@gmail.com"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Sakib",
                    LastName = "Hasan",
                    UserName = "sakib",
                    Password = "R@dif2022",
                    Role = "user",
                    Email = "sakib@gmail.com"
                });
        }
    }
}
