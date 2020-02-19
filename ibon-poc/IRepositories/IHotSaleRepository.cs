using System;
using System.Threading.Tasks;
using ibon_poc.Models.DBModels;

namespace ibon_poc.IRepositories
{
    public interface IHotSaleRepository : IDisposable
    {
        Task<HotSale> GetById(int id);
    }
}
