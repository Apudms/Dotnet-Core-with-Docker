using System.Data.SqlClient;
using WebAPI.Contracts;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class ProductService : IProduct
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;
        private readonly SqlConnection _connection;
        private SqlCommand _command;

        public ProductService(IConfiguration config) 
        {
            _config = config;
            _connectionString = _config.GetConnectionString("ConnStr");
            _connection = new SqlConnection(_connectionString);
        }

        #region Add Product using Store Procedure
        public Product Add(Product entity)
        {
            try
            {
                string query = @"sp_InsertProduct;SELECT @@IDENTITY";
                _command = new SqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@ProductName", entity.ProductName);
                _command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
                _command.Parameters.AddWithValue("@Stock", entity.Stock);
                _command.Parameters.AddWithValue("@Price", entity.Price);
                _connection.Open();
                entity.ProductId = Convert.ToInt32(_command.ExecuteScalar());
                return entity;
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException(sqlEx.Message);
            }
            finally
            {
                _command.Dispose();
                _connection.Close();
            }
        }
        #endregion

        #region 
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public IEnumerable<Product> GetByCategoryName(string categoryName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public IEnumerable<Product> GetByProductName(string productName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public int GetProductStock(int productId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public IEnumerable<Product> GetProductWithCategory()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 
        public Product Update(Product entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
