using InvoiceSystem.Application.Documents;
using InvoiceSystem.Application.Dtos;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Services
{
    public class PdfService
    {
        public byte[] GenerateInvoicePdf(OrderResponse order)
        {
            var document = new InvoiceDocument(order);
            return document.GeneratePdf();
        }
    }
}
