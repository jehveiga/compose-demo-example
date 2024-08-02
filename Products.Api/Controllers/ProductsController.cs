using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Products.Api.Data;
using Products.Api.Dtos.Requests;
using Products.Api.Dtos.Responses;
using Products.Api.Entities;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            var productResponse = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price)).ToList();

            return Ok(productResponse);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(Guid id, IDistributedCache cache)
        {
            var product = await cache.GetAsync($"products-{id}",
                async token =>
                {
                    var product = await _context.Products.AsNoTracking()
                                                   .FirstOrDefaultAsync(p => p.Id == id, token);

                    return product;
                },
                CacheOptions.DefaultExpiration);

            if (product == null)
            {
                return NotFound();
            }

            var productResponse = ProductResponse.FromtEntity(product);

            return productResponse;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, UpdateProductRequest productRequest, IDistributedCache cache)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.AtualizarProduto(productRequest.Name, productRequest.Price);

            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            await cache.RemoveAsync($"products-{id}");

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(AddProductRequest productRequest)
        {
            var product = productRequest.ToEntity();

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productResponse = ProductResponse.FromtEntity(product);

            return CreatedAtAction("GetProduct", new { id = productResponse.Id }, productResponse);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, IDistributedCache cache)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            await cache.RemoveAsync($"products-{id}");

            return NoContent();
        }
    }
}
