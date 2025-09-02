using APITest.Data;
using APITest.DTO;
using APITest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ItemsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> AllItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> AllItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"Item.Id {id} not exists");
            }
            return Ok(item);
        }
        [HttpGet("AllItemWithCategory/{idCategory}")]
        public async Task<IActionResult> AllItemWithCategory(int idCategory)
        {
            var item = await _db.Items.Where(x => x.CategoryId == idCategory).ToListAsync();
            if (item == null)
            {
                return NotFound($"Category.Id {idCategory} has no items");
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] DtoItems di)
        {
            using var stream = new MemoryStream();
            await di.Image.CopyToAsync(stream);
            var item = new Item
            {
                Name = di.Name,
                Price = di.Price,
                Notes = di.Notes,
                CategoryId = di.CategoryId,
                Image = stream.ToArray()
            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);
            
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] DtoItems di)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound($"item id {id} not exists !");
            }
            var isCategoryExists = await _db.Categories.AnyAsync(x => x.Id == di.CategoryId);
            if (isCategoryExists)
            {
                return NotFound($"item id {id} not exists !");
            }
            if (di.Image != null)
            {
                using var stream = new MemoryStream();
                await di.Image.CopyToAsync(stream);
                item.Image = stream.ToArray();
            }
            item.Name = di.Name;
            item.Price = di.Price;
            item.Notes = di.Notes;
            item.CategoryId = di.CategoryId;

            _db.SaveChanges();
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"item id {id} not exists !");
            }
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }
    }
}
