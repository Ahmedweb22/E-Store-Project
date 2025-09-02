using APITest.Data;
using APITest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db)
        {
             _db = db;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _db.Categories.ToList();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetCategories(int id)
        {
            var category = _db.Categories.SingleOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound($"Category.Id {id} not exists");
            }
            return Ok(category);
        }
        [HttpPost]
        public IActionResult AddCategory(string category)
        { 
        Category c = new() { Name = category };
            _db.Categories.Add(c);
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpPut]
        public IActionResult UpdateCategory(Category category)
        {
           var c = _db.Categories.SingleOrDefault(x => x.Id == category.Id);
            if (c == null)
            {
                return NotFound($"Category.Id {category.Id} not exists");
            }
            c.Name = category.Name;
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateCategoryPatch([FromBody] JsonPatchDocument<Category> category , [FromRoute] int id)
        {
            var c = _db.Categories.SingleOrDefault(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category.Id {id} not exists");
            }
            category.ApplyTo(c);
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpDelete("{id}")]
        public IActionResult RemoveCategory(int id)
        {
            var c = _db.Categories.SingleOrDefault(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category.Id {id} not exists");
            }
            _db.Categories.Remove(c);
            _db.SaveChanges();
            return Ok(c);
        }

    }
}
