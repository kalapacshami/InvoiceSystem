using InvoiceSystem.Application.Dtos;
using InvoiceSystem.Application.Services;
using InvoiceSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly PdfService _pdfService;

        public OrdersController(OrderService orderService, PdfService pdfService)
        {
            _orderService = orderService;
            _pdfService = pdfService;
        }

        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order is null)
                return NotFound();

            var pdfBytes = _pdfService.GenerateInvoicePdf(order);
            return File(pdfBytes, "application/pdf", $"invoice-order-{id}.pdf");
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order is null)
                return NotFound();

            return Ok(order);
        }
    }
}
