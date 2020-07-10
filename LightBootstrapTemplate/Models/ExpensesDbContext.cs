using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightBootstrapTemplate.Models
{
    public class ExpensesDbContext:DbContext
    {
        //public ExpensesDbContext() : base("ExpensesDbContextLocal")
        //{

        //}

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Income> Income { get; set;}
        public DbSet<Diary> Diaries { get; set; }
        public DbSet<Goal> Goals { get; set; }
    }
}