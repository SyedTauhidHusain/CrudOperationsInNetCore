using CrudOperationsInNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationsInNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _dbContext;

        public BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }
        //to get all record 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrand()
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            return await _dbContext.Brands.ToListAsync();
        }

        //get all records by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        // add new record
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new {id = brand.ID},brand);
        }

        //update existing record
        [HttpPut]
        public async Task<IActionResult> PutBrand(Brand brand)
        {
            
            _dbContext.Entry(brand).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(brand.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            }
            return Ok();
        }
        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        //delete record
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            _dbContext.Brands.Remove(brand);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }



    }
}
