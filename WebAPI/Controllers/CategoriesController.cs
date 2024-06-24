using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebAPI.Interfaces;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoriesController(ICategory category)
        {
            _category = category;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var categories = _category.GetAll();
            return categories;
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            var category = _category.GetById(id);
            return category;
        }

        // GET api/<CategoriesController>/5
        [HttpGet("ByName")]
        public IEnumerable<Category> GetByName(string name)
        {
            var categories = _category.GetByCategoryName(name);
            return categories;
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public ActionResult Post(Category category)
        {
            try
            {
                var result = _category.Add(category);
                return CreatedAtAction(nameof(Get),
                    new { id = category.CategoryId },
                    category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Category category)
        {
            var updateCateg = _category.GetById(id);
            try
            {
                if (updateCateg != null)
                {
                    updateCateg.CategoryName = category.CategoryName;
                    var result = _category.Update(updateCateg);
                    return Ok(result);
                }
                return BadRequest($"Category {category.CategoryName} has not been updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deleteCateg = _category.GetById(id);
                if (deleteCateg != null)
                {
                    _category.Delete(deleteCateg.CategoryId);
                    return Ok($"Data Category ID {id} deleted");
                }
                return BadRequest($"Data Category ID {id} has not been deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
