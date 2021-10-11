using Microsoft.EntityFrameworkCore;
using MyAspProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyAspProject.Data
{
    public class MyAspDbContext : DbContext 
{
        public MyAspDbContext(DbContextOptions<MyAspDbContext> options) : base(options)
        {

        }
        public DbSet <Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
