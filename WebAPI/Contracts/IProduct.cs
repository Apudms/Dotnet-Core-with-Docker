using WebAPI.Models;

namespace WebAPI.Contracts
{
    public interface IProduct : ICrud<Product>
    {
        IEnumerable<Product> GetByProductName(string productName);
        IEnumerable<Product> GetByCategoryName(string categoryName);
        IEnumerable<Product> GetProductWithCategory();
        int GetProductStock(int productId);
    }
}
