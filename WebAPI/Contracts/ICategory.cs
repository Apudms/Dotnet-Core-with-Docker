using WebAPI.Models;

namespace WebAPI.Contracts
{
    public interface ICategory : ICrud<Category>
    {
        IEnumerable<Category> GetByCategoryName(string categoryName);
    }
}
