using System.Data.SqlClient;
using CatalogAPI.DAL.Interfaces;
using CatalogAPI.DTO;
using CatalogAPI.Models;
using Dapper;

namespace CatalogAPI.DAL
{
    public class DetailDapper : IJoinTable
    {
        private readonly IConfiguration _config;
        private readonly Connection _conn;

        public DetailDapper(IConfiguration config)
        {
            _config = config;
            _conn = new Connection(_config);
        }

        public IEnumerable<DetailDto> GetByName(string name)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"SELECT p.Name, p.[Description], p.Price,p.Quantity, c.CategoryName FROM Products p, Categories c WHERE p.CategoryID = c.CategoryID AND Name LIKE @Name";
                var param = new { Name = $"%{name}%" };
                try
                {
                    var product = conn.Query<DetailDto>(strSql, param);
                    return product;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public IEnumerable<DetailDto> GetDetailProducts()
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                try
                {
                    var strSql = @"SELECT c.CategoryID, p.ProductID, p.Name, p.[Description], p.Price,p.Quantity, c.CategoryName FROM Products p, Categories c WHERE p.CategoryID = c.CategoryID";
                    var details = conn.Query<DetailDto>(strSql);
                    if (details == null)
                    {
                        throw new ArgumentException("Data Null");
                    }
                    return details;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }
    }
}