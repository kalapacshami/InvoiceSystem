using InvoiceSystem.Application.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InvoiceSystem.Application.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly OrderResponse _order;
        public InvoiceDocument(OrderResponse order)
        {
            _order = order;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }
        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("INVOICE").FontSize(20).Bold();
                    column.Item().Text($"Order #{_order.Id}");
                    column.Item().Text($"Date: {_order.OrderDate:yyyy-MM-dd}");
                });

                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Item().Text("Customer").Bold();
                    column.Item().Text(_order.CustomerName);
                });
            });
        }
        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(10);

                column.Item().Element(ComposeItemsTable);

                column.Item().AlignRight().Text($"TOTAL: {_order.TotalAmount:0.00}")
                    .FontSize(14).Bold();
            });
        }

        private void ComposeItemsTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Product
                    columns.RelativeColumn(1); // Qty
                    columns.RelativeColumn(1); // Unit price
                    columns.RelativeColumn(1); // Line total
                    columns.RelativeColumn(2); // Flags
                });

                table.Header(header =>
                {
                    header.Cell().Text("Product").Bold();
                    header.Cell().Text("Qty").Bold();
                    header.Cell().Text("Unit Price").Bold();
                    header.Cell().Text("Line Total").Bold();
                    header.Cell().Text("Flags").Bold();

                    header.Cell().ColumnSpan(5).PaddingTop(5)
                        .BorderBottom(1).BorderColor(Colors.Black);
                });

                foreach (var item in _order.Items)
                {
                    table.Cell().Text(item.ProductName);
                    table.Cell().Text(item.Quantity.ToString());
                    table.Cell().Text(item.UnitPrice.ToString("0.00"));
                    table.Cell().Text(item.LineTotal.ToString("0.00"));

                    var flags = new List<string>();
                    if (item.IsDiscountEligible) flags.Add("Discount");
                    if (item.IsHazardous) flags.Add("Hazardous");
                    table.Cell().Text(flags.Count > 0 ? string.Join(", ", flags) : "-");
                }
            });
        }
    }
}
