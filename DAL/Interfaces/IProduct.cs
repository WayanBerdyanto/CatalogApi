using CatalogAPI.Models;

namespace CatalogAPI.DAL.Interfaces
{
    public interface IProduct : ICrud<Product>
    {
        IEnumerable<Product> GetByName(string name);

        Task UpdateStockAsync(int id, int quantity);
    }
}