using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WT_Lab.API.Data;
using WT_Lab.Domain;
using WT_Lab.Models;

namespace WT_Lab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseData<IEnumerable<Category>>>> GetCategories()
        {
            var response = new ResponseData<IEnumerable<Category>>
            {
                Data = await _context.Category.ToListAsync()
            };
            return response;
        }
        //public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        //{
        //    return await _context.Category.ToListAsync();
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var Category = await _context.Category.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return Category;
        }
        [HttpPut]
        public async Task<IActionResult> PutCategory(int id, Category Category)
        {
            if (id != Category.ID)
            {
                return BadRequest();
            }
            _context.Entry(Category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else { throw; }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category Category)
        {
            _context.Category.Add(Category);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCategory", new { id = Category.ID }, Category);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var Category = await _context.Category.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            _context.Category.Remove(Category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(a => a.ID == id);
        }

    }
}
