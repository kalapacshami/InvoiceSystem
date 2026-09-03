using InvoiceSystem.Application.Dtos;
using InvoiceSystem.Application.Settings;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace InvoiceSystem.Application.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly decimal _discountPercentage;
        public OrderService(AppDbContext context, IOptions<DiscountSettings> discountSettings)
        {
            _context = context;
            _discountPercentage = discountSettings.Value.DiscountPercentage;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };
            foreach (var item in request.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product is null)
                    throw new InvalidOperationException($"Product with id {item.ProductId} does not exist.");
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPriceAtOrderTime = product.UnitPrice
                });
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(order.Id)
                ?? throw new InvalidOperationException("Order was created but could not be reloaded.");
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return null;

            return MapToResponse(order);
        }

        private OrderResponse MapToResponse(Order order)
        {
            var items = order.OrderItems.Select(oi =>
            {
                var lineSubtotal = oi.Quantity * oi.UnitPriceAtOrderTime;
                var lineTotal = oi.Product.IsDiscountEligible
                    ? lineSubtotal * (1 - _discountPercentage / 100m)
                    : lineSubtotal;

                return new OrderItemResponse(
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Quantity,
                    oi.UnitPriceAtOrderTime,
                    lineTotal,
                    oi.Product.IsHazardous,
                    oi.Product.IsDiscountEligible
                );
            }).ToList();

            var total = items.Sum(i => i.LineTotal);

            return new OrderResponse(
                order.Id,
                order.OrderDate,
                order.CustomerId,
                order.Customer.Name,
                items,
                total
            );
        }
    }
}