using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Dtos
{
    public record CreateOrderItemRequest(int ProductId, int Quantity);

    public record CreateOrderRequest(int CustomerId, List<CreateOrderItemRequest> Items);
}
