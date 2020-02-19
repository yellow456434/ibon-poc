using System;
using System.Threading.Tasks;
using ibon_poc.IRepositories;
using ibon_poc.Models;
using ibon_poc.Models.DBModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ibon_poc.Repositories
{
    public class HotSaleRepository : IHotSaleRepository
    {
        private readonly BookDbContext bookDbContext;

        public HotSaleRepository(BookDbContext bookDbContext)
        {
            this.bookDbContext = bookDbContext;
        }

        public void Dispose() => bookDbContext.Dispose();

        public async Task<HotSale> GetById(int id)
        => await (from item in bookDbContext.HotSale.AsNoTracking()
                  where item.id == id
                  select item).FirstOrDefaultAsync();
    }
}
