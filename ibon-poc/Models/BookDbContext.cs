using System;
using ibon_poc.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace ibon_poc.Models
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<HotSale> HotSale {set;get;}
    }
}
