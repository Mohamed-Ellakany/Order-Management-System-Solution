using Microsoft.EntityFrameworkCore;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Repository.Data
{
    public class OrderManagementDbContext : DbContext
    {
        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Customer> Customers { get; set; }


        public DbSet<Order> Orders { get; set; }


        public DbSet<OrderItem> OrderItems { get; set; }


        public DbSet<Product> Products { get; set; }

         
        public DbSet<Invoice> Invoices { get; set; }




    }
}
