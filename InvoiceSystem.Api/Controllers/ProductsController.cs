using InvoiceSystem.Application.Services;
using InvoiceSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] CreateProductRequest request)
        {
            var product = await _productService.CreateProductAsync(
                request.Name, request.Category, request.UnitPrice,
                request.IsHazardous, request.IsDiscountEligible);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
    }

    public record CreateProductRequest(
        string Name, string Category, decimal UnitPrice, bool IsHazardous, bool IsDiscountEligible);
}
