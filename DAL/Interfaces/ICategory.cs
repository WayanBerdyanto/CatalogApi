using CatalogAPI.Models;

namespace CatalogAPI.DAL.Interfaces
{
    public interface ICategory : ICrud<Category>
    {
        IEnumerable<Category> GetByName(string name);
    }
}