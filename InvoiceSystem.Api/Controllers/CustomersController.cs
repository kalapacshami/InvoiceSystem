using InvoiceSystem.Application.Services;
using InvoiceSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create([FromBody] CreateCustomerRequest request)
        {
            var customer = await _customerService.CreateCustomerAsync(
                request.Name, request.Country, request.Address);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }
    }

    public record CreateCustomerRequest(string Name, string Country, string Address);
}
