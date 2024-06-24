using Microsoft.Data.SqlClient;
using System.Data;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class CategoriesDAL : ICategory
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;

        public CategoriesDAL(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("ConnStr");
            _connection = new SqlConnection(_connectionString);
        }

        #region Add
        public Category Add(Category entity)
        {
            try
            {
                string query = @"INSERT INTO Categories(CategoryName)
                                VALUES(@CategoryName); SELECT @@IDENTITY";

                _command = new SqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@CategoryName", entity.CategoryName);
                _connection.Open();

                // Menjalankan Perintah Insert
                int lastCategoryId = Convert.ToInt32(_command.ExecuteScalar());
                entity.CategoryId = lastCategoryId;
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

        #region Delete
        public void Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM Categories
                                WHERE CategoryId = @CategoryId";

                _command = new SqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@CategoryId", id);
                _connection.Open();

                int result = _command.ExecuteNonQuery();
                if (result != 1)
                {
                    throw new ArgumentException($"Failed to delete data. No item with ID {id} found in the database.");
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException("An error occurred while accessing the database. Please try again later.", sqlEx);
            }
            finally
            {
                _command.Dispose();
                _connection.Close();
            }
        }
        #endregion

        #region Get All
        public IEnumerable<Category> GetAll()
        {
            try
            {
                List<Category> categories = new List<Category>();
                string query = @"sp_GetAllCategories";
                _command = new SqlCommand(query, _connection);
                _command.CommandType = CommandType.StoredProcedure;
                _connection.Open();
                _reader = _command.ExecuteReader();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = Convert.ToInt32(_reader["CategoryId"]),
                            CategoryName = _reader["CategoryName"].ToString(),
                        });
                    }
                }
                _reader.Close();
                return categories;
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message}");
            }
            finally
            {
                _command.Dispose();
                _connection.Close();
            }
        }
        #endregion

        #region Get By ID
        public Category GetById(int id)
        {
            try
            {
                Category category = new Category();
                string query = @"SELECT * FROM Categories
                                WHERE CategoryId = @CategoryId";

                _command = new SqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@CategoryId", id);
                _connection.Open();
                _reader = _command.ExecuteReader();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        category.CategoryId = Convert.ToInt32(_reader["CategoryId"]);
                        category.CategoryName = _reader["CategoryName"].ToString();
                    }
                }
                _reader.Close();
                return category;
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message}");
            }
            finally
            {
                _command.Dispose();
                _connection.Close();
            }
        }
        #endregion

        #region Get By Name
        public IEnumerable<Category> GetByCategoryName(string categoryName)
        {
            try
            {
                Category category = new Category();
                string query = @"SELECT * FROM Categories
                                 WHERE CategoryName LIKE @CategoryName";

                _command = new SqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@CategoryName", "%" + categoryName + "%");
                _connection.Open();
                _reader = _command.ExecuteReader();

                List<Category> categories = new List<Category>();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = Convert.ToInt32(_reader["CategoryID"]),
                            CategoryName = _reader["CategoryName"].ToString()
                        });
                    }
                }
                _reader.Close();
                return categories;
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message}");
            }
            finally
            {
                _command.Dispose();
                _connection.Close();
            }
        }
        #endregion

        #region Update
        public Category Update(Category entity)
        {
            try
            {
                Category category = new Category();
                string query = @"UPDATE Categories 
                                 SET CategoryName = @CategoryName 
                                 WHERE CategoryId = @CategoryId";

                _command = new SqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@CategoryName", entity.CategoryName);
                _command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
                _connection.Open();

                int result = _command.ExecuteNonQuery();
                if (result == 1)
                {
                    return entity;
                }
                else
                {
                    throw new ArgumentException("Failed to update data. Please check your input.");
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Database error occurred: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error occurred: {ex.Message}");
            }
            finally
            {
                _command.Dispose();
                _connection.Close();
            }
        }
        #endregion
    }
}
