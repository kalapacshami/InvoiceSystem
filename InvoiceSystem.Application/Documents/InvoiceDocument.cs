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
    }
}
