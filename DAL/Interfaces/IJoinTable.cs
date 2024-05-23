using CatalogAPI.DTO;
using CatalogAPI.Models;

namespace CatalogAPI.DAL.Interfaces
{
    public interface IJoinTable : IJoin<DetailDto>
    {
        IEnumerable<DetailDto> GetByName(string name);
    }
}