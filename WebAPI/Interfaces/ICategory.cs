using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface ICategory : ICrud<Category>
    {
        IEnumerable<Category> GetByCategoryName(string categoryName);
    }
}
