using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Dtos
{
    public record OrderItemResponse(
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    bool IsHazardous,
    bool IsDiscountEligible);

    public record OrderResponse(
        int Id,
        DateTime OrderDate,
        int CustomerId,
        string CustomerName,
        List<OrderItemResponse> Items,
        decimal TotalAmount);
}
