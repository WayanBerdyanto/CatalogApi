namespace CatalogAPI.DAL.Interfaces
{
    public interface IJoin<T>
    {
        IEnumerable<T> GetDetailProducts();
    }
}