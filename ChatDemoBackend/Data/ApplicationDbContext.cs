using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatDemoBackend.Models;

/*
    Here we shoud mention all tables in the database  
*/

namespace ChatDemoBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

        public DbSet<User_Info> User_Info { get; set; }

    }
}
